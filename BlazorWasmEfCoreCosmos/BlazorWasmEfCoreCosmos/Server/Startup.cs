using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using BlazorWasmEfCoreCosmos.Database;
using System;

namespace BlazorWasmEfCoreCosmos.Server {
  public class Startup {
    public Startup(IConfiguration configuration) {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services) {
      /* TODO: Create an entry in user secrets
       *   "AzureCosmos": {
       *     "AccountEndpoint": "fill in the Uri value from the Azure Cosmos Keys page",
       *     "AccountKey": "fill in the Primary Key value from the Azure Cosmos Keys page"
       *   }
       */
      services.AddDbContext<PortfolioContext>(options => options.UseCosmos(
         accountEndpoint: Configuration["AzureCosmos:AccountEndpoint"],
         accountKey: Configuration["AzureCosmos:AccountKey"],
         databaseName: Configuration["AzureCosmos:Database"]));

      services.AddControllersWithViews();
      services.AddRazorPages();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider) {
      if (env.IsDevelopment()) {
        app.UseDeveloperExceptionPage();
        app.UseWebAssemblyDebugging();
        EnsureDatabaseCreated(serviceProvider);
      } else {
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
      }

      app.UseHttpsRedirection();
      app.UseBlazorFrameworkFiles();
      app.UseStaticFiles();

      app.UseRouting();

      app.UseEndpoints(endpoints => {
        endpoints.MapRazorPages();
        endpoints.MapControllers();
        endpoints.MapFallbackToFile("index.html");
      });
    }

    private async void EnsureDatabaseCreated(IServiceProvider serviceProvider) {
      using var scope = serviceProvider.CreateScope();
      using var context = scope.ServiceProvider.GetService<PortfolioContext>();
      await context.Database.EnsureCreatedAsync();
    }
  }
}

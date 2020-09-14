using BlazorWasmEfCoreCosmos.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorWasmEfCoreCosmos.Database {

  /// <summary>
  /// The Entity Framework Core Database Context for the Portfolio
  /// Collection.
  /// </summary>
  /// <remarks>
  /// This class should be registered in the Startup.cs class of your 
  /// application using:
  /// <code>
  /// services.AddDbContext<CosmosDbContext>(options => options.UseCosmos(
  ///     accountEndpoint: Configuration.GetValue<string>("AzureCosmos:AccountEndpoint"),
  ///     accountKey: Configuration.GetValue<string>("AzureCosmos:AccountKey"),
  ///     databaseName: CosmosDbContext.DbName));
  /// </code>
  /// It is recommended that you, at minimum, store the appropriate values in User Secrets
  /// instead of appsettings.json.  For production, it is recommended ot use Azure Key Vault
  /// or other secure store.
  /// Resources
  ///  - https://dotnetcoretutorials.com/2020/05/02/using-azure-cosmosdb-with-net-core-part-2-ef-core/
  ///  - https://docs.microsoft.com/en-us/ef/core/providers/cosmos/?tabs=dotnet-core-cli
  ///  - https://docs.microsoft.com/en-us/aspnet/core/security/key-vault-configuration?view=aspnetcore-3.1
  ///  - https://docs.microsoft.com/en-us/azure/cosmos-db/local-emulator-release-notes
  /// </remarks>
  public class PortfolioContext : DbContext {
    #region Constructors
    /// <summary>
    /// Initialize a new PortfolioContext
    /// </summary>
    public PortfolioContext() { }

    /// <summary>
    /// Initialize a new PortfolioContext
    /// </summary>
    /// <param name="options">The settings for the database connection</param>
    public PortfolioContext(DbContextOptions<PortfolioContext> options)
      : base(options) { }
    #endregion

    #region Entities
    /// <summary>
    /// The Project records
    /// </summary>
    public DbSet<Project> Projects { get; set; }
    #endregion

    #region Overridden Methods
    /// <summary>
    /// Configure the database (and other options) to be used for this context
    /// </summary>
    /// <param name="optionsBuilder">
    /// A builder used to create or modify options for this context. Databases (and other
    /// extensions) typically define extension methods on this object that allow you
    /// to configure the context.
    /// </param>
    /// <remarks>
    /// Descriptions are from the base DbContext class
    /// </remarks>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
      if (!optionsBuilder.IsConfigured) {
        optionsBuilder.UseCosmos("localhost", "", "Portfolio");
      }
    }

    /// <summary>
    /// Further configure the model that was discovered by convention
    /// from the entity types exposed in Microsoft.EntityFrameworkCore.DbSet`1 properties
    /// on your derived context. The resulting model may be cached and re-used for subsequent
    /// instances of your derived context.
    /// </summary>
    /// <param name="modelBuilder">
    /// The builder being used to construct the model for this context. Databases (and
    /// other extensions) typically define extension methods on this object that allow
    /// you to configure aspects of the model that are specific to a given database.
    /// </param>
    /// <remarks>
    /// Descriptions are from the base DbContext class
    /// </remarks>
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.Entity<Project>(entity => {
        entity.ToContainer("Project");
        entity.Property(e => e.Id).ToJsonProperty("Id");
        entity.Property(e => e.Name).ToJsonProperty("Name");
        entity.Property(e => e.StartDate).ToJsonProperty("StartDate");
        entity.Property(e => e.EndDate).ToJsonProperty("EndDate");
        entity.Property(e => e.Url).ToJsonProperty("Url");
        entity.OwnsMany(
          e => e.Comments,
          c => {
            c.ToJsonProperty("Comments");
            c.Property(e => e.Description).ToJsonProperty("Description");
            c.Property(e => e.CreatedDate).ToJsonProperty("CreatedDate");
          }
        );
      });
    }
    #endregion
  }
}

# blazorwasm-efcore-cosmos
Blazor WASM Application with Entity Framework Core using Azure Cosmos.  The sample application is a *very* simple Project Portfolio management system.  The database consists of one document type (a Project) that can have multiple Comment types added to it.

## Introduction
This repository is to show examples of how to use Microsoft Azure CosmosDB (CoreSQL API) with Entity Framework Core in a .NET Blazor Web Assembly (WASM) application that has uses ASP.NET hosted server.  In the future, I plan on adding patterns for using the MongoDB API. Also worth noting is that the Entity Framework Core setup will work for the Blazor Server flavor as well.

## Prerequisites
In time, I hope to make this more verbose, but for now, I will leave out some of the details pertaining to creating a new Blazor WASM application. 
 - Either the [Azure CosmosDB Emulator](https://aka.ms/cosmosdb-emulator) or Azure CosmosDB service with an Account that uses the CoreSQL API
 - Familiarity with the Azure Portal
 - Familiarity with referencing NuGet packages in either Visual Studio or using the .NET Core CLI
 - .NET Core SDK >=3.1 and < .NET 5 (I have not tested this on the .NET 5 preview version)
 - Familiarity with cloning or downloading a GitHub repository, creating a new Blazor WASM solution, or an existing Blazor WASM solution
 - Recommended to use Visual Studio 2019 Community, but this will work with VS Code, or 

## Getting Started
There are many ways to get started with Blazor. If you are unfamiliar with creating a new .NET Blazor solution, I recommend you visit the [Blazor Home Page](https://blazor.net) to get started with understanding what Blazor is and how to get started.  The solution in this repository is the scaffolded code from using Visual Studio 2019 Community Edition and the latest LTS version of the .NET Core (3.1.402 at the time I created the solution). If you follow the getting started tutorial on the [Blazor Home Page](https://blazor.net), you will create a new Blazor Server application using the .NET Core CLI. The steps described below for connecting to CosmosDB should be the same.

If you want to skip the walkthrough for setting this up as a new project, you can jump to either the section for Add the Database Models and Context or Inspecting the Repository's Solution sections below. Otherwise, continue reading to find out how to recreate this project starting from File -> New Project, or using the .NET Core CLI.

## Create a New Blazor WASM Solution

### From Visual Studio
1. Open Visual Studio
2. If you see the Get started dialog, then click **Create a new project**, otherwise use the menu bar to navigate to **File -> New -> Project**
3. From the New Project dialog, choose **Blazor App** from the list
>*Note: You can filter the list using the search input at the top of it, as well as the dropdowns. Make sure that if anything other than __All__ is selected in the dropdowns, that they are __C#__, __Linux__, __macOS__, __Windows__, __Cloud__, and/or __Web__.  If you still do not see the __Blazor app__ option, you may need to install the [lates LTS of .NET Core](https://dot.net).*
4. Name your project and choose a destination
>*Note: If you currently have a project open, then you will see the options for __Create a New Solution__ or __Add to Solution__.  If you intend to add the Blazor projects to your existing solution, select __Add to Solution__, otherwise select __Create a New Solution__.*
5. Click __Create__
6. On the Create a new Blazor app dialog, choose __Blazor WebAssembly App__
7. At minimum, select the __ASP.NET Core hosted__ option on the right
8. Click __Create__

### Using the .NET Core CLI
1. Create and navigate to the location you would like the solution to be created in
2. Run the command
```powershell
dotnet new blazorwasm -ho -o AppName
```
>*Note: Replace the "AppName" text above with the name you would like to use for the solution and project prefixes. This name will appear as the first part of projects in the solution. The sample code's solution in thie repository is named "BlazorWasmEfCoreCosmos".*
3. Open the solution in either VS Code or Visual Studio

## Add the Database Models and Context
While is it okay to use the existing projects for the Models and Database Context classes, when there is a possibility that the same Entity Framework Database Context and Models could be used across multiple projects, I prefer to separate the Models and the Context into a separate solution and generate a NuGet package that the applications use.  In development, I set up the application project with a reference to the Database projects.  For the purposes of this sample, I am separating the Models and Context into two class library projects.  If you do not want to create separate projects, use the scaffolded "\*.Shared" project for the models and the "\*.Server" project for the Database Context.  If you are starting from a Blazor Server project, then simply use the base Project for both the Models and Context.  Feel free to skip this section and go to the Create the Models section below. 

### Create Separate Projects for the Database Models and Context
1. Right click on the Solution item in the Solution Explorer and navigate to **Add -> New Project**
>*Note: If the Solution Explorer is not showing, use the menu bar and navigate to __View -> Solution Explorer__ or press `CTRL + ALT + L`. You could also use the menu bar to navigate to __File -> New -> Project__.  Just make sure to select the option to __Add to Solution__ on the second screen.*
2. Choose **Class Library (.NET Standard)** from the list and click **Create**
3. Type "*{SolutionName}*.Models" for the Project Name
>*Note: You can name the project anything, but when creating the project in the same solution, I found it easier to name it the same as the solution with __.Models__ and __.Database__ appended to the end.*
4. Click **Create**
5. Delete the scaffolded Class1.cs file
6. Repeat steps 1-5, using a project name of "*{SolutionName}*.Database"

### Add Project References for the Database Models and Context
1. Right click on the *{SolutionName}*.Database project in the Solution Explorer and navigate to **Add -> Project Reference**
>*Note: If you do not see "Project Reference" as an option, choose __Reference__ instead.  Then, click on the __Projects__ item on the left of the Reference Manager dialog.*
2. Place a checkmark next to the *{SolutionName}*.Models project
3. Click __Ok__
4. Repeat steps 1-3 for the *{SolutionName}*.Client and *{SolutionName}*.Shared projects
>*Note: The Client and Shared projects do not currently use the models, but this will make them available to the two projects.*
5. Repeat steps 1 and 2 for the *{SolutionName}*.Server project
6. Place a checkmark next to the *{SolutionName}*.Database project
7. Click __Ok__

### Add the Entity Framework Core Reference to the Database and Server Projects
There are a few ways to add references to NuGet packages.  Below uses Visual Studio's built-in NuGet Package Manager.  Find other ways to install and manage NuGet packages in the References section.

1. Use the menu bar to navigate to **Tools -> NuGet Package Manager -> Manage NuGet Packages for Solution...**
2. In the NuGet Package Manager window, click on the **Browse** item at the top
3. In the Search input, type "Microsoft.EntityFrameworkCore.Cosmos"
4. Select the __Microsoft.EntityFrameworkCore.Cosmos__ item
5. In the Projects panel on the right, place a checkmark next to the *{SolutionName}*.Database and *{SolutionName}*.Server projects
6. Click __Install__
7. Read and Accept the terms and click __Ok__ to any dialogs that popup
>*Note: If you do not accept the terms, you will not be able to complete this sample.*
8. Close the NuGet Package Manager tab

### Create the Models
1. Right click on the *{SolutionName}*.Models project in the Solution Explorer and navigate to **Add -> New Item**
>*Note: You could use Keyboard Shortcut `CTRL + SHIFT + A` to add items to the currently selected container item in the Solution Explorer.  For example, if you have a file selected inside of a folder, the new file will be created in the same folder.*
2. Select __Class__ from the list of types
3. In the Name field, type "Project.cs"
4. Click "Add"
5. Repeat steps 1-4, using a Name of "Comment.cs"
6. If it is not already open, open the Comment.cs file
7. Change the class signature line to read:
```csharp
public partial class Comment
```
8. In the class body, add the following properties
```csharp
public string Description { get; set; }
public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
```
9. Save and close the Comment.cs file
10. If it is not already open, open the Project.cs file
11. Change the class signature line to read:
```csharp
public partial class Project
```
8. In the class body, add the following properties
```csharp
public Guid Id { get; set; }
public string Name { get; set; }
public string Description { get; set; }
public DateTime? StartDate { get; set; }
public DateTime? EndDate { get; set; }
public string Url { get; set; }

public List<Comment> Comments { get; set; }
```
9. Save and close the Project.cs file

### Create the Database Context Class
1. Right click on the *{SolutionName}*.Models project in the Solution Explorer and navigate to **Add -> New Item**
>*Note: You could use Keyboard Shortcut `CTRL + SHIFT + A` to add items to the currently selected container item in the Solution Explorer.  For example, if you have a file selected inside of a folder, the new file will be created in the same folder.*
2. Select __Class__ from the list of types
3. In the Name field, type "Portfolio.Context.cs"
4. Click "Add"
5. If the file is not already open, open the Portfolio.Context.cs file
6. Change the class signature line to read:
```csharp
public partial class PortfolioContext
```
7. Inside of the class body, add the following code:
```csharp
// A default, parameterless constructor for the context
public PortfolioContext() { }

/// <summary>
/// Initialize a new PortfolioContext
/// </summary>
/// <param name="options">The settings for the database connection</param>
/// <remarks>
/// This is used when registering the DbContext for the application in the 
/// application's Startup.cs file (or Program.cs if not using a Startup.cs)
/// </remarks>
public PortfolioContext(DbContextOptions<PortfolioContext> options)
  : base(options) { }

// The set of Documents that are of type Project
public DbSet<Project> Projects { get; set; }

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
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
{
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
protected override void OnModelCreating(ModelBuilder modelBuilder) 
{
    // Start configuring the Project entity
    modelBuilder.Entity<Project>(entity => 
    {
        // Bind the data retrieved from the Project container to the Project.cs model
        entity.ToContainer("Project");
        // Set each property's binding to the appropriate Json property
        entity.Property(e => e.Id).ToJsonProperty("Id");
        entity.Property(e => e.Name).ToJsonProperty("Name");
        entity.Property(e => e.StartDate).ToJsonProperty("StartDate");
        entity.Property(e => e.EndDate).ToJsonProperty("EndDate");
        entity.Property(e => e.Url).ToJsonProperty("Url");
        // Establish the hierarchy for one Project document owning many Comment records
        entity.OwnsMany(
            e => e.Comments,
            c => 
            {
                c.ToJsonProperty("Comments");
                c.Property(e => e.Description).ToJsonProperty("Description");
                c.Property(e => e.CreatedDate).ToJsonProperty("CreatedDate");
              }
        );
    });
}
```
8. Review the comments for each item in the code for more information about what they do
9. Save and close the Portfolio.Context.cs file

## Setup the Blazor Application's DbContext Reference
1. Find and open the *{SolutionName}*.Server project's Startup.cs file in the Solution Explorer
2. Locate the `ConfigureServices` method
3. Add the following code to the start of the method body
```csharp
services.AddDbContext<PortfolioContext>(options => options.UseCosmos(
     accountEndpoint: Configuration["AzureCosmos:AccountEndpoint"],
     accountKey: Configuration["AzureCosmos:AccountKey"],
     databaseName: Configuration["AzureCosmos:Database"]));
```
4. Either use the recommended fixes from the light bulb to add the references to the *{SolutionName}*.Database project and Microsoft.EntityFrameworkCore.Cosmos package, or add the following using statements to the top of the Starup.cs file, replacing *{SolutionName}* with your solution name
```csharp
using Microsoft.EntityFrameworkCore;
using {SolutionName}.Database;
```
5. Save and close the Startup.cs file

## Add the AzureCosmos settings to appsettings.json or User Secrets
1. Either find and open the *{SolutionName}*.Server project's appsettings.json file or right click on the project and choose __Manage User Secrets__
2. Add the following Key to the json file, replacing the AccountEndpoint, AccountKey, and DatabaseName with the appropriate values from the Keys section of the Azure CosmosDB portal page
```json
"AzureCosmos": 
{
    "AccountEndpoint": "https://localhost:{emulatorPort}",
    "AccountKey": "{AccountKey}",
    "DatabaseName": "{DbName}"
},
```
3. Save and close either the appsettings.json or User Secrets file

## TODO: Add an API Controller for simple CRUD Operations

## TODO: Add a sample page that uses the HttpClient to send requests to the API Controller

# Inspecting the Repository's Solution
If you want to see the full setup, clone or download this repo.  I recommend starting by inspecting the .csproj files for the BlazorWasmEfCoreCosmos.Database and BlazorWasmEfCoreCosmos.Server projects. Note that the only added Nuget package is 
```xml
<PackageReference Include="Microsoft.EntityFrameworkCore.Cosmos" Version="3.1.8" />
```
Next, view the BlazorWasmEfCoreCosmos.Models project's `Project.cs` and `Comment.cs` to see the POCOs that represent the two entities that shape the document data for the database.  Then, head over to the BlazorWasmEfCoreCosmos.Database project's `Portfolio.Context.cs` file to see how the Database Context that will be exposed to the application gets configured.  Finally, head to the BlazorWasmEfCoreCosmos.Server's `Startup.cs` to see how to add the CosmosDB's Database Context to the application.

## Resources
 - [.NET Core SDKs](https://dot.net)
 - [Blazor Home Page](https://blazor.net)
 - [Azure Downloads](https://azure.microsoft.com/en-us/downloads/?sdk=net)
 - [Azure CosmosDB Getting Started](https://docs.microsoft.com/en-us/azure/cosmos-db/introduction)
 - [Azure CosmosDB Emulator](https://aka.ms/cosmosdb-emulator)
 - [Blazor Getting Started Tutorial](https://dotnet.microsoft.com/learn/aspnet/blazor-tutorial/intro)
 - [DevExpress Blazor Training Videos](https://dotnet.microsoft.com/learn/aspnet/blazor-tutorial/intro)
 - [Blazor University](https://blazor-university.com/)
 - [AwesomeBlazor GitHub Repo](https://aka.ms/awesomeblazor)
 - [Steve Sanderson's Car Checker Demo App](https://aka.ms/blazor-carchecker)
 - [Install and Manage NuGet packages in Visual Studio using the NuGet Package Manager](https://docs.microsoft.com/en-us/nuget/consume-packages/install-use-packages-visual-studio)
 - [Install and Manage NuGet packages in Visual Studio for Mac](https://docs.microsoft.com/en-us/visualstudio/mac/nuget-walkthrough?toc=%2Fnuget%2Ftoc.json&view=vsmac-2019)
 - [Manage NuGet packages Using the nuget.exe CLI](https://docs.microsoft.com/en-us/nuget/consume-packages/install-use-packages-nuget-cli)
 - [Manage NuGet packages Using the .NET Core CLI](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-add-package)
 - 

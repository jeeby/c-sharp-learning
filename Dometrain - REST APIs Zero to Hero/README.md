# Dometrain - Rest APIs Zero to Hero

## 1. Creating New Project

### API Layer
* ASP.NET Web Application
    * Web API Type
    * Name: ProjectName.API
    * References Application and Contracts projects

### Application Layer
* Class Library
    * Name: Movies.Application
    * Will contain all business and application logic
    * Could be service layer

### Contracts Project
* Class Library
    * Name: Movies.Contracts
    * Keep API contracts here
        * Can publish to nuget etc for external use

## 2. Define API Contracts
* Create folders in Contracts project
    * Requests
        * C# Class for each request (eg: CreateMovieRequest)
        * Model that matches the objects being used by the API (eg: Movie)
            ```csharp
           namespace Movies.Contracts.Requests

            public class CreateMovieRequest {
                public required string Title {get; init;}
                public required int YearOfRelease {get; init;}
                public required IEnumerable<string> Genres {get; init;} = Enumerable.Empty<string>();
            }
            ```
            _Note: `required` - property is mandatory | 
             `init` - property can't be changed during runtime_
            
    * Responses
        ```csharp
        namespace Movies.Contracts.Responses

        public class MovieResponse {
            public required Guid Id {get; init; }
            public required string Title {get; init;}
            public required int YearOfRelease {get; init;}
            public required IEnumerable<string> Genres {get; init;} = Enumerable.Empty<string>();
        }
        ```
* Don't reuse classes across contracts, because you may need to change them for some, and once you've published them you can't change them or it will break the contract

## 3. Create in-memory database
* For this example, we're creating an in-memory representation of a DB. This could be any type of DB really.
* Repository folder to store the repositories (data access code)
    * Class and interface for each one (eg: `MovieRepository` and `IMovieRepository`)
        * Added some basic CRUD API Endpoints in there
* Domain model required. Models folder stores these.
    * Movie model, with same properties as contract
        ```csharp
        namespace Movies.Application.Models

        public class Movie {
            public required Guid Id { get; init; } // init: can't change at runtime
            public required string Title { get; set; }
            public required int YearOfRelease { get; set; }
            public required List<string> Genres { get; init; } = new();
        }
        ```
* Register the repository with the application
    * Could do by adding the following to `Program.cs`, however this means the application needs to know about the repo, which is not a good design:
        ```csharp
        builder.Services.AddSingleton<IMovieRepository, MovieRepository>();
        ```
    * Better option is to add an extension method in Program.cs, and implement it in the Application layer:
        ```csharp
        builder.AddApplication();
        ```
        * In our Application project, we can install the `Microsoft.Extensions.DependencyInjection.Abstractions` nuget package

## 4. Create a Controller

* Create controller: `MoviesController`
    * Extends `ControllerBase`
    * Has `[ApiController]` attribute
    * Add a `[Route("api")]` parameter/ attribute
        * Could do `api/movies` but there's a better way to do this
    * Inject our `IMovieRepository`

* Create controller method: `Create`, `Get`, `GetAll`, `Update`, `Delete`
    * Set the relevant contract (eg: `CreateMovieRequest`) as the parameter
    * Give it a route type attribute, eg:`[HttpPost("movies")]`
    * Call repository to get/create/update/delete movie
    * Returns `201 Created` using `Created("/api/movies/{movie.Id}", movie)`, NOT just `Ok(movie`)
    * Nice improvement is to move the API paths to a static class (eg: `ApiEndpoints.cs`)
    * Another good improvement is to use the `CreateAtAction` return method to return the 201
        * `CreatedAtAction(nameof(Get), new { id = movie.Id }, movie);`
        * This will return the full path (including hostname), which is pretty handy

* Create a mapping class: `ContractMapping`
    * Keep all mappings in here (as extension methods)
    * No business logic in here, just mapping

* What about `Patch`
    * Patch fallen out of favour lately due to the complexity of implementing partial updates
    * Easier for client to use `GET`, mutate object, then use `PUT` to save changes
    
## 5. Implement slug-based retrieval

Slugs are useful when you need to provide a more memorable path to your item instead of requiring IDs (eg: guids) to get them.

* Add `Slug` as a computed property on the `Movie` class
    ```csharp 
    public string Slug => GenerateSlug();
* Generate slug class just does some string manipulation to generate a string like this:: `my-movie-title-2024`
    * Source generation of Regex makes it faster, eg:
        ```csharp
        [GeneratedRegex("[^0-9A-Za-z _-]", RegexOptions.NonBacktracking, 5)]
        private static partial Regex SlugRegex();
        ```

* Add to `MovieResponse` class, and add to mapping.
* Add a try parse to the `GET` controller, and a new repository method to get by Slug, and determine which one to call
    ```csharp
    var movie = Guid.TryParse(idOrSlug, out var id)
        ? await _movieRepository.GetByIdAsync(id)
        : await _movieRepository.GetBySlugAsync(idOrSlug);
    ```
## 6. Moving to a real database

We are going to setup Postgres in a docker container in order to use as database storage.

We could have used Entity Framework, however that would negate the need for a repository layer, 
which we still want for the purposes of these lessons. So we are going to use Dapper instead.

### Create Docker database

* Create the `docker-compose.yml` file in `Movies.Api` project.
* Navigate to folder with docker compose file, and in terminal run `docker compose up`.
* When running, connect to DB via IDE, using credentials in docker compose file.

### Add database infrastructure code

* Create `DbConnectionFactory.cs` in `Movies.Application` project
  * Implements `IDbConnectionFactory`
  * Receive connectionString in constructor
  * Create a `CreateConnectionAsync()` function that returns the connection
  * Create extension method to register connection factory in `ApplicationServicesCollectionExtensions.cs`
  * Register in `Movies.API/Program.cs` 
    * Also add Database connectionString to `appsettings.json` and retrieve from there
* Create `DbInitializer.cs` file in `Movies.Application` project
  * Add some database initialization code in here
  * Add to extension method to register initializer in `ApplicationServiceCollectionExtensions.cs`
  * Register in `Movies.API/Program.cs`, at the end after Controllers are mapped
    ```csharp
    var dbInitializer = app.Services.GetRequiredService<DbInitializer>();
    await dbInitializer.InitializeAsync();
    ```
    
### Remove old in-memory database

* Just need to change the movie repository implementation
  * Remove code from all the repository functions
  * Inject connection factory into `MovieRepository`
  * For each function:
    * Make `async`
    * Add `using var connection = await _dbConnectionFactory.CreateConnectionAsync();`
    * If updating more than 1 table, add `using var transaction = connection.BeginTransaction();`
    * Execute DB query, eg:
      ```csharp
      var result = await connection.ExecuteAsync(new CommandDefinition($"""
                        delete from movies where id = @id
                        """, new { id }));
      ```
    * Commit transaction: `transaction.Commit();` where created
    * Return bool result: `return result > 0;`

### Adding a business logic layer

We don't want the controller talking directly to the repository. Therefore, we are  going to need business logic to live somewhere
    
* Not in repository as that should only be for database interactivity
* Shouldn't be in controller because then business logic is coupled to API
* Having a service layer allows other applications to interact with our system

#### Create the service layer

* Create a business logic layer in a Services folder in the Application
  * If doing Clean Architecture, we would use Mediator pattern to handle the services
  * Also could have Dto objects (eg: MovieDto) that get returned by the service, with a mapper in the application layer
    * Just using application model all the way through for this app for simplicity
* Forward calls to the repository layer, making chnages as required (eg: update to return Movie object instead of bool)
* Inject service into Controller instead of Repository
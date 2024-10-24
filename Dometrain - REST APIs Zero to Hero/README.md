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

## 3. Create Database
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
    
using Microsoft.Extensions.DependencyInjection;
using Movies.Application.Database;
using Movies.Application.Repositories;

namespace Movies.Application;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection  AddApplication(this IServiceCollection services)  
    {
        // This is just abstractions of dependencies, not the actual implementation
        services.AddSingleton<IMovieRepository, MovieRepository>();
        return services;
    }

    // Register the DB Connection Factory
    public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
    {
        // Singletons as these are only going to be used once
        services.AddSingleton<IDbConnectionFactory>(_ => new NpgsqlConnectionFactory(connectionString));
        services.AddSingleton<DbInitializer>();
        return services;
    }
}
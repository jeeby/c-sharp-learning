using Microsoft.Extensions.DependencyInjection;
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
}
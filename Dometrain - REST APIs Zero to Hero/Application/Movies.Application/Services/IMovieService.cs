using Movies.Application.Models;

namespace Movies.Application.Services;

public interface IMovieService
{
    Task<bool> CreateAsync(Movie movie);
    Task<Movie?> GetByIdAsync(Guid id); // Nullable return type
    Task<Movie?> GetBySlugAsync(string slug); 
    Task<IEnumerable<Movie>> GetAllAsync();
    Task<Movie?> UpdateAsync(Movie movie);
    Task<bool> DeleteByIdAsync(Guid id); // Often ById is omitted as it's implied
}
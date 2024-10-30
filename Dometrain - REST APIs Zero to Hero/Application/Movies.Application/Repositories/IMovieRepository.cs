using Movies.Application.Models;

namespace Movies.Application.Repositories;

public interface IMovieRepository {
    Task<bool> CreateAsync(Movie movie);
    Task<Movie?> GetByIdAsync(Guid id); // Nullable return type
    Task<Movie?> GetBySlugAsync(string slug); 
    Task<IEnumerable<Movie>> GetAllAsync();
    Task<bool> UpdateAsync(Movie movie);
    Task<bool> DeleteByIdAsync(Guid id); // Often ById is omitted as it's implied
    Task<bool> ExistsByIdAsync(Guid id);
}
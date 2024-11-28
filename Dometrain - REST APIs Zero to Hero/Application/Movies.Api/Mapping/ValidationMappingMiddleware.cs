using System.ComponentModel.DataAnnotations;

namespace Movies.Api.Mapping;

public class ValidationMappingMiddleware
{
    private readonly RequestDelegate _next;

    public ValidationMappingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task HandleAsync(HttpContext context)
    {
        try
        {
            // pass the request to the next step in the pipeline
            await _next(context);
        }
        catch (ValidationException ex)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
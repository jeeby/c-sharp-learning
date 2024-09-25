// See https://aka.ms/new-console-template for more information
using MultiProject.ClassLibrary;
using System.Data.Entity;

Console.WriteLine("Hello, World!");

NicksPublicClass nicksPublicClass = new();
nicksPublicClass.SayHello();

NicksPublicClassWithInternalMembers nicksPublicClassWithInternalMembers = new();

// Internal method is normally not available, but have been made available via csproj attribute: InternalsVisibleToAttribute
nicksPublicClassWithInternalMembers.InternalMethod();

// Internal class is normally not available, but have been made available via csproj attribute: InternalsVisibleToAttribute
NicksInternalClass nicksInternalClass = new();
nicksInternalClass.InternalMethod();

// Demo of using code from a referenced (via nuget) package (Entity Framework)
public class AppContext : DbContext
{
    public DbSet<Product> Products => Set<Product>();
}

public record Product(
    int Id,
    string Name,
    string Description,
    decimal Price);
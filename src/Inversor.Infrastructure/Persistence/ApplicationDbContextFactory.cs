
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Inversor.Infrastructure.Persistence;

/// <summary>
/// Design-time factory for EF Core migrations tooling.
/// Bypasses Web API host startup validations when running 'dotnet ef' commands.
/// </summary>
public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        // Fallback connection string for local EF Core CLI design-time operations
        var connectionString = "Host=localhost;Port=5432;Database=inversor_db;Username=postgres;Password=postgres";
        optionsBuilder.UseNpgsql(connectionString);
        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
using Inversor.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Inversor.Api.Extensions;

public static class MigrationExtensions
{
    public static async Task ApplyProjectMigrationsAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        int retries = 5;
        while (retries > 0)
        {
            try
            {
                var pendingMigrations = await context.Database.GetPendingMigrationsAsync();

                if (pendingMigrations.Any())
                {
                    await context.Database.MigrateAsync();
                }
                else
                {
                    logger.LogInformation("Database is up to date. No migrations required.");
                }

                break; 
            }
            catch (Exception ex)
            {
                retries--;
                logger.LogWarning(ex, "Fail connect to database. Remaining attempts: {Retries}", retries);
                if (retries == 0)
                {
                    logger.LogError("Critical error: Could not verify or migrate the database.");

                    throw;
                }
                await Task.Delay(3000);
            }
        }
    }
}
using Inversor.Core.Application.Abstractions;
using Inversor.Infrastructure.AI.Services;
using Inversor.Infrastructure.Persistence;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Inversor.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IApplicationDbContext>(provider =>
            provider.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<IAiEvaluatorService, GeminiEvaluatorService>();

        // MassTransit configuration with RabbitMQ
        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((context, cfg) =>
            {
                var host = configuration["RabbitMQ:Host"] ?? "localhost";
                var port = ushort.TryParse(configuration["RabbitMQ:Port"], out var p) ? p : (ushort)5672;
                var username = configuration["RabbitMQ:Username"] ?? "guest";
                var password = configuration["RabbitMQ:Password"] ?? "guest";
                cfg.Host(host, port, "/", h =>
                {
                    h.Username(username);
                    h.Password(password);
                });
                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}

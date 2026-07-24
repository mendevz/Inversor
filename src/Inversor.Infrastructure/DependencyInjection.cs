using Inversor.Core.Application.Abstractions;
using Inversor.Infrastructure.AI.Services;
using Inversor.Infrastructure.Options;
using Inversor.Infrastructure.Persistence;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Inversor.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, 
        IConfiguration configuration,
        Action<IBusRegistrationConfigurator>? configureMassTransit = null)
    {

        // Strongly-typed options registration with startup validation
        services.AddOptions<GeminiOptions>()
            .BindConfiguration(GeminiOptions.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<RabbitMqOptions>()
            .BindConfiguration(RabbitMqOptions.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        var connectionString = configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("Connection string 'DefaultConnection' was not found or is empty.");
        }

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IApplicationDbContext>(provider =>
            provider.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<IAiEvaluatorService, GeminiEvaluatorService>();

        // MassTransit configuration with RabbitMQ
        services.AddMassTransit(x =>
        {

            x.AddEntityFrameworkOutbox<ApplicationDbContext>(o =>
            {
                o.UsePostgres();
                o.UseBusOutbox();
            });

            configureMassTransit?.Invoke(x);

            x.UsingRabbitMq((context, cfg) =>
            {
                var rabbitOptions = context.GetRequiredService<IOptions<RabbitMqOptions>>().Value;
                var host = rabbitOptions.Host;
                var port = rabbitOptions.Port;
                var username = rabbitOptions.Username;
                var password = rabbitOptions.Password;

                cfg.Host(host, port, "/", h =>
                {
                    h.Username(username);
                    h.Password(password);
                });

                cfg.UseMessageRetry(r =>
                {
                    r.Interval(2, TimeSpan.FromSeconds(30));
                });

                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}

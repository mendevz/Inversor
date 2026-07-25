using Inversor.Core.Application.Abstractions;
using Inversor.Infrastructure.AI.Services;
using Inversor.Infrastructure.Options;
using Inversor.Infrastructure.Persistence;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;

namespace Inversor.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, 
        IConfiguration configuration,
        Action<IBusRegistrationConfigurator>? configureMassTransit = null)
    {
        services.AddIOptions();

        // -- Add Database Configuration --
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException("Connection string 'DefaultConnection' was not found or is empty.");

        services.AddDbContext<ApplicationDbContext>(
            options => options.UseNpgsql(connectionString)
        );

        services.AddScoped<IApplicationDbContext>(
            provider => provider.GetRequiredService<ApplicationDbContext>()
        );

        // --------------------------------

        services.AddScoped<IAiEvaluatorService, GeminiEvaluatorService>();
        services.AddMessaginConfiguration(configureMassTransit);
        services.AddOpenTelemetryInversor(configuration);

        return services;
    }

    private static IServiceCollection AddMessaginConfiguration(
        this IServiceCollection services, 
        Action<IBusRegistrationConfigurator>? configureMassTransit = null)
    {
        services.AddMassTransit(x =>
        {
            // -- Outbox pattern configuration --
            x.AddEntityFrameworkOutbox<ApplicationDbContext>(o =>
            {
                o.UsePostgres();
                o.UseBusOutbox();
            });

            // -- Add Consumers from the assembly --
            configureMassTransit?.Invoke(x);

            // -- Configure RabbitMQ --
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

                // -- Retry configuration for message consumption failures --
                cfg.UseMessageRetry(r =>
                {
                    r.Interval(2, TimeSpan.FromSeconds(30));
                });

                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }

    private static IServiceCollection AddOpenTelemetryInversor(this IServiceCollection services, IConfiguration configuration)
    {
        var serviceName = configuration["OTEL_SERVICE_NAME"] ?? "Inversor";

        services.AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService(serviceName))
            .WithTracing(tracing =>
            {
                // -- Configure tracing sources --
                tracing
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddEntityFrameworkCoreInstrumentation(options =>
                    {
                        // -- Filter out specific queries from being traced --
                        options.Filter = (providerName, dbCommand) =>
                            !dbCommand.CommandText.Contains("InboxState", StringComparison.OrdinalIgnoreCase) &&
                            !dbCommand.CommandText.Contains("OutboxMessage", StringComparison.OrdinalIgnoreCase) &&
                            !dbCommand.CommandText.Contains("OutboxState", StringComparison.OrdinalIgnoreCase);
                    })
                    .AddSource("MassTransit")
                    .AddOtlpExporter();
            }).WithMetrics(metrics =>
            {
                metrics
                    .AddMeter("Microsoft.AspNetCore.Hosting")        // Métricas HTTP (RPS, latencias, peticiones activas)
                    .AddMeter("Microsoft.AspNetCore.Server.Kestrel") // Métricas del servidor web local
                    .AddMeter("System.Net.Http")                     // Métricas salientes hacia la IA de Gemini
                    .AddMeter("MassTransit")                         // Métricas de consumo y publicación en RabbitMQ
                    .AddOtlpExporter();
            });

        // -- Configure logging --
        services.AddLogging(logging =>
        {
            logging.AddOpenTelemetry(options =>
            {
                options.IncludeFormattedMessage = true;
                options.IncludeScopes = true;

                var resourceBuilder = ResourceBuilder.CreateDefault().AddService(serviceName);
                options.SetResourceBuilder(resourceBuilder);

                options.AddOtlpExporter();
            });
        });

        return services;
    }

    private static IServiceCollection AddIOptions(this IServiceCollection services)
    {
        // -- Strongly-typed options registration with startup validation --
        services.AddOptions<GeminiOptions>()
            .BindConfiguration(GeminiOptions.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<RabbitMqOptions>()
            .BindConfiguration(RabbitMqOptions.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return services;
    }
}

using Inversor.Core.Application.UseCases;
using Microsoft.Extensions.DependencyInjection;

namespace Inversor.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<EvaluateTranslationUseCase>();
        return services;
    }
}
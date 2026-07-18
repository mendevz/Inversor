using Microsoft.Extensions.DependencyInjection;

namespace Inversor.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return services;
    }
}
using Microsoft.Extensions.DependencyInjection;

namespace SourceGeneration.ChangeTracking;

public static class ChangeTrackingStateServiceCollectionExntesions
{
    public static IServiceCollection AddState<T>(this IServiceCollection services, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped) where T : State<T>
    {
        var type = typeof(T);
        services.Add(new ServiceDescriptor(type, type, serviceLifetime));
        services.AddTransient(p => p.GetRequiredService<T>().CreateTracker());
        return services;
    }
}

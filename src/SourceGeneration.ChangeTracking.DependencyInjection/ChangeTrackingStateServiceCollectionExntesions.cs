using Microsoft.Extensions.DependencyInjection;

namespace SourceGeneration.ChangeTracking;

public static class ChangeTrackingStateServiceCollectionExntesions
{
    public static IServiceCollection AddState<T>(this IServiceCollection services, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped) where T : State<T>, new()
    {
        services.Add(new ServiceDescriptor(typeof(T), _ => ChangeTrackingProxyFactory.Create(new T()), serviceLifetime));
        return services;
    }
}

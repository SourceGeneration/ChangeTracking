using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace SourceGeneration.States;

public static class SourceGenerationStateServiceCollectionExtensions
{
    private static readonly List<ServiceDescriptor> _services = [];

    public static IServiceCollection AddStateInjection(this IServiceCollection services)
    {
        foreach (var service in _services)
            services.Add(service);
        return services;
    }

    public static IServiceCollection AddState<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.PublicConstructors)] TState>(
        this IServiceCollection services,
        TState state,
        ServiceLifetime serviceLifetime = ServiceLifetime.Scoped) where TState : class
    {
        AddStateCore(services, state, serviceLifetime);
        return services;
    }

    public static IServiceCollection AddState<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.PublicConstructors)] TState>(
        this IServiceCollection services, 
        ServiceLifetime serviceLifetime = ServiceLifetime.Scoped) where TState : class
    {
        AddStateCore<TState>(services, null, serviceLifetime);
        return services;
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void AddState<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.PublicConstructors)] TState>(ServiceLifetime serviceLifetime = ServiceLifetime.Scoped) where TState : class
    {
        AddStateCore<TState>(_services, null, serviceLifetime);
    }

    private static void AddStateCore<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.PublicConstructors)] TState>(
        IList<ServiceDescriptor> services,
        TState? state,
        ServiceLifetime serviceLifetime) where TState : class
    {
        services.Add(new ServiceDescriptor(typeof(TState), _ => ChangeTrackingProxyFactory.Create(state ?? Activator.CreateInstance<TState>()), serviceLifetime));
        services.Add(new ServiceDescriptor(typeof(State<TState>), p => new State<TState>(p.GetRequiredService<TState>()), serviceLifetime));
        services.Add(new ServiceDescriptor(typeof(IScopedState<TState>), p => new State<TState>(p.GetRequiredService<State<TState>>()), ServiceLifetime.Transient));
    }
}

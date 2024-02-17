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
        {
            services.Add(service);
        }
        return services;
    }

    public static IServiceCollection AddState<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.PublicConstructors)] TState>(this IServiceCollection services, ServiceLifetime storeLifetime = ServiceLifetime.Scoped) where TState : class
    {
        if (storeLifetime == ServiceLifetime.Singleton)
        {
            services.AddSingleton(_ => ChangeTrackingProxyFactory.Create(Activator.CreateInstance<TState>()));
            services.AddSingleton<State<TState>, State<TState>>();
        }
        else
        {
            services.AddScoped(_ => ChangeTrackingProxyFactory.Create(Activator.CreateInstance<TState>()));
            services.AddScoped<State<TState>, State<TState>>();
        }
        services.AddTransient<IState<TState>, State<TState>>(services => new State<TState>(services.GetRequiredService<State<TState>>()));
        return services;
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void Add<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.PublicConstructors)] TState>(ServiceLifetime lifetime = ServiceLifetime.Scoped) where TState : class
    {
        _services.Add(new ServiceDescriptor(typeof(TState), _ => ChangeTrackingProxyFactory.Create(Activator.CreateInstance<TState>()), lifetime));
        _services.Add(new ServiceDescriptor(typeof(State< TState>), p => new State<TState>(p.GetRequiredService<TState>()), lifetime));
        _services.Add(new ServiceDescriptor(typeof(IState<TState>), p => new State<TState>(p.GetRequiredService<State<TState>>()), ServiceLifetime.Transient));
    }

}

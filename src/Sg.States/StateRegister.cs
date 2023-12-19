using Microsoft.Extensions.DependencyInjection;
using SourceGeneration.States;
using System.ComponentModel;

namespace SourceGeneration.States;

[EditorBrowsable(EditorBrowsableState.Never)]
public static class StateRegister
{
    private static readonly List<ServiceDescriptor> _services = [];

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void Add<TState>() where TState : class
    {
        _services.Add(new ServiceDescriptor(typeof(TState), _ => ChangeTrackingProxyFactory.Create(Activator.CreateInstance<TState>()), ServiceLifetime.Scoped));
        _services.Add(new ServiceDescriptor(typeof(IStore<TState>), p => new State<TState>(p.GetRequiredService<TState>()), ServiceLifetime.Scoped));
        _services.Add(new ServiceDescriptor(typeof(IState<TState>), p => new State<TState>((State<TState>)p.GetRequiredService<IStore<TState>>()), ServiceLifetime.Transient));
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void Register(IServiceCollection services)
    {
        foreach (var service in _services)
        {
            services.Add(service);
        }
    }
}
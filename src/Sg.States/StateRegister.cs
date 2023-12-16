using Microsoft.Extensions.DependencyInjection;
using Sg.ChangeTracking;

namespace Sg.States;

public static class StateRegister
{
    private static readonly List<ServiceDescriptor> _services = [];

    public static void Add<TState>() where TState : class
    {
        _services.Add(new ServiceDescriptor(typeof(TState), _ => ChangeTrackingProxyFactory.Create(Activator.CreateInstance<TState>()), ServiceLifetime.Scoped));
        _services.Add(new ServiceDescriptor(typeof(IStore<TState>), p => new State<TState>(p.GetRequiredService<TState>()), ServiceLifetime.Scoped));
        _services.Add(new ServiceDescriptor(typeof(IState<TState>), p => new State<TState>((State<TState>)p.GetRequiredService<IStore<TState>>()), ServiceLifetime.Transient));
    }

    public static void Register(IServiceCollection services)
    {
        foreach (var service in _services)
        {
            services.Add(service);
        }
    }
}
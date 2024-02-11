using Microsoft.Extensions.DependencyInjection;

namespace SourceGeneration.States;

public static class SgStateServiceCollectionExtensions
{
    public static IServiceCollection AddState<TState>(this IServiceCollection services, ServiceLifetime storeLifetime = ServiceLifetime.Scoped) where TState : class
    {
        if (storeLifetime == ServiceLifetime.Singleton)
        {
            services.AddSingleton(_ => ChangeTrackingProxyFactory.Create(Activator.CreateInstance<TState>()));
            services.AddSingleton<IStore<TState>, State<TState>>();
        }
        else
        {
            services.AddScoped(_ => ChangeTrackingProxyFactory.Create(Activator.CreateInstance<TState>()));
            services.AddScoped<IStore<TState>, State<TState>>();
        }
        services.AddTransient<IState<TState>, State<TState>>(services => new State<TState>((State<TState>)services.GetRequiredService<IStore<TState>>()));
        return services;
    }

}

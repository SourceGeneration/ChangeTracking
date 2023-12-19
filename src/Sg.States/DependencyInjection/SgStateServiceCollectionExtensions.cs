using Microsoft.Extensions.DependencyInjection;

namespace SourceGeneration.States;

public static class SgStateServiceCollectionExtensions
{
    //public static IServiceCollection AddState(this IServiceCollection services)
    //{
    //    StateRegister.Register(services);
    //    return services;
    //}

    public static IServiceCollection AddState<TState>(this IServiceCollection services) where TState : class
    {
        services.AddScoped(_ => ChangeTrackingProxyFactory.Create(Activator.CreateInstance<TState>()));
        services.AddScoped<IStore<TState>, State<TState>>();
        services.AddTransient<IState<TState>, State<TState>>(services => new State<TState>((State<TState>)services.GetRequiredService<IStore<TState>>()));
        return services;
    }
}

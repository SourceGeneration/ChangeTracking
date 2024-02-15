namespace SourceGeneration.States;

public interface IState : IDisposable
{
    IDisposable SubscribeBindingChanged(Action next);
}

public interface IState<TState> : IState, IObservable<TState>
{
    TState Value { get; }
    IDisposable Bind<TValue>(Func<TState, TValue> selector, Action<TValue> subscriber, ChangeTrackingScope distinctUntilChanged = ChangeTrackingScope.RootChanged);
    IDisposable Bind<TValue>(Func<TState, TValue> selector, Action<TValue> subscriber, IEqualityComparer<TValue> comparer);
    IDisposable SubscribeBindingChanged(Action<TState> next);
}

public interface IStore<TState> : IState<TState>
{
    //void Set(TState state);
    void Update(Action<TState> action);
}

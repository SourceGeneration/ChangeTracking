using System.Diagnostics.CodeAnalysis;

namespace SourceGeneration.States;

public interface IState : IDisposable
{
    IDisposable SubscribeBindingChanged(Action next);
}

public interface IState<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.PublicConstructors)] TState> : IState, IObservable<TState>
{
    TState Value { get; }
    IDisposable Bind<TValue>(Func<TState, TValue> selector, Action<TValue> subscriber, IEqualityComparer<TValue> comparer);
    IDisposable Bind<TValue>(Func<TState, TValue> selector, Func<TValue, bool>? predicate, Action<TValue> subscriber, IEqualityComparer<TValue> equalityComparer);
    IDisposable Bind<TValue>(Func<TState, TValue> selector, Action<TValue> subscriber, ChangeTrackingScope scope = ChangeTrackingScope.Root);
    IDisposable Bind<TValue>(Func<TState, TValue> selector, Func<TValue, bool>? predicate, Action<TValue> subscriber, ChangeTrackingScope scope = ChangeTrackingScope.Root);
    IDisposable SubscribeBindingChanged(Action<TState> next);
}
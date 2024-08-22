using System;

namespace SourceGeneration.ChangeTracking;

public interface IChangeTracker : IDisposable
{
    IDisposable OnChange(Action subscriber);
    void AcceptChanges();
}

public interface IChangeTracker<TState> : IChangeTracker where TState : class
{
    IDisposable Watch<TValue>(Func<TState, TValue> selector, ChangeTrackingScope scope = ChangeTrackingScope.Root);
    IDisposable Watch<TValue>(Func<TState, TValue> selector, Action<TValue>? subscriber, ChangeTrackingScope scope = ChangeTrackingScope.Root);
    IDisposable Watch<TValue>(Func<TState, TValue> selector, Func<TValue, bool>? predicate, ChangeTrackingScope scope = ChangeTrackingScope.Root);
    IDisposable Watch<TValue>(Func<TState, TValue> selector, Func<TValue, bool>? predicate, Action<TValue>? subscriber, ChangeTrackingScope scope = ChangeTrackingScope.Root);
    IDisposable OnChange(Action<TState> subscriber);
}
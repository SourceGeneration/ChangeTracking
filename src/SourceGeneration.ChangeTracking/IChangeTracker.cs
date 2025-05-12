using System;
using System.Collections.Generic;
using System.Collections.Specialized;

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

    IDisposable Watch<TItem>(Func<TState, ChangeTrackingList<TItem>> selector, Func<TItem, bool> predicate, Action<IEnumerable<TItem>>? subscriber = null, ChangeTrackingScope scope = ChangeTrackingScope.Root);
    IDisposable Watch<TCollection, TItem>(Func<TState, TCollection> selector, Func<TItem, bool> predicate, Action<IEnumerable<TItem>>? subscriber = null, ChangeTrackingScope scope = ChangeTrackingScope.Root) where TCollection : IEnumerable<TItem>, INotifyCollectionChanged;

    IDisposable OnChange(Action<TState> subscriber);
}
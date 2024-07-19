using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Linq;

namespace SourceGeneration.ChangeTracking;

public class ChangeTracker<TState> : IChangeTracker<TState> where TState : class
{
    private ImmutableArray<IChangeTracking> _watches = [];
    private ImmutableArray<Action<TState>> _subscribers = [];
    private readonly Action<ChangeTracker<TState>>? _disposeAction;

    public ChangeTracker(TState state)
    {
        State = state;
    }

    internal ChangeTracker(TState state, Action<ChangeTracker<TState>> disposeAction)
    {
        State = state;
        _disposeAction = disposeAction;
    }

    public TState State { get; }

    public void AcceptChanges()
    {
        lock (State)
        {
            var watches = _watches;
            foreach (var observer in watches.Cast<IObserver<TState>>())
            {
                observer.OnNext(State);
            }

            if (watches.Any(x => x.IsChanged))
            {
                var subscribes = _subscribers;
                foreach (var subscriber in subscribes)
                {
                    subscriber(State);
                }
            }

            foreach (IChangeTracking observer in watches)
            {
                observer.AcceptChanges();
            }
        }
    }

    public IDisposable Watch<TValue>(Func<TState, TValue> selector, ChangeTrackingScope scope = ChangeTrackingScope.Root)
    {
        return Watch(selector, null, null, scope);
    }

    public IDisposable Watch<TValue>(Func<TState, TValue> selector, Action<TValue>? subscriber, ChangeTrackingScope scope = ChangeTrackingScope.Root)
    {
        return Watch(selector, null, subscriber, scope);
    }

    public IDisposable Watch<TValue>(Func<TState, TValue> selector, Func<TValue, bool>? predicate, ChangeTrackingScope scope = ChangeTrackingScope.Root)
    {
        return Watch(selector, predicate, null, scope);
    }

    public IDisposable Watch<TValue>(Func<TState, TValue> selector, Func<TValue, bool>? predicate, Action<TValue>? subscriber, ChangeTrackingScope scope = ChangeTrackingScope.Root)
    {
        var subscription = new Subscription<TValue>(State, selector, predicate, subscriber, new ChangeTrackingScopeEqualityComparer<TValue>(scope));
        _watches = _watches.Add(subscription);
        return new Disposable(() => _watches = _watches.Remove(subscription));
    }

    public IDisposable OnChange(Action subscriber) => OnChange(_ => subscriber());

    public IDisposable OnChange(Action<TState> subscriber)
    {
        _subscribers = _subscribers.Add(subscriber);
        return new Disposable(() => _subscribers = _subscribers.Remove(subscriber));
    }

    public void Dispose()
    {
        _subscribers = _subscribers.Clear();
        _watches = _watches.Clear();
        _disposeAction?.Invoke(this);
    }

    private sealed class Subscription<TValue> : IObserver<TState>, IChangeTracking
    {
        private readonly Func<TState, TValue> _selector;
        private readonly Func<TValue, bool>? _predicate;
        private readonly Action<TValue>? _subscriber;
        private readonly IEqualityComparer<TValue> _equalityComparer;
        private TValue _value = default!;
        private bool _changed;

        public Subscription(
            TState state,
            Func<TState, TValue> selector,
            Func<TValue, bool>? predicate,
            Action<TValue>? subscriber,
            IEqualityComparer<TValue> equalityComparer)
        {
            _selector = selector;
            _predicate = predicate;
            _subscriber = subscriber;
            _equalityComparer = equalityComparer;

            if (SetValue(state))
            {
                subscriber?.Invoke(_value);
            }
        }

        public bool IsChanged => _changed;

        public void AcceptChanges()
        {
            _changed = false;
        }

        public void OnCompleted() { }
        public void OnError(Exception error) { }

        public void OnNext(TState value)
        {
            if (SetValue(value))
            {
                _changed = true;
                _subscriber?.Invoke(_value);
            }
        }

        private bool SetValue(TState value)
        {
            var select = _selector(value);

            if (_predicate == null || _predicate(select))
            {
                if (!_equalityComparer.Equals(_value, select))
                {
                    _value = select;
                    return true;
                }
            }
            return false;
        }
    }

    private sealed class Disposable(Action callback) : IDisposable
    {
        private bool _disposed;

        public void Dispose()
        {
            if (!_disposed)
            {
                callback();
                _disposed = true;
            }
        }
    }
}

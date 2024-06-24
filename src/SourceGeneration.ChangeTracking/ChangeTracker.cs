using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace SourceGeneration.ChangeTracking;

public class ChangeTracker<TState> : IChangeTracker<TState> where TState : class
{
    private readonly List<IChangeTracking> _watches = [];
    private readonly List<Action<TState>> _subscribers = [];
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
        foreach (var observer in _watches.Cast<IObserver<TState>>())
        {
            observer.OnNext(State);
        }

        if (_watches.Any(x => x.IsChanged))
        {
            foreach (var subscriber in _subscribers)
            {
                subscriber(State);
            }
        }

        foreach (IChangeTracking observer in _watches)
        {
            observer.AcceptChanges();
        }

        if (State is IChangeTracking tracking)
        {
            tracking.AcceptChanges();
        }
    }

    public IDisposable Watch<TValue>(Func<TState, TValue> selector, ChangeTrackingScope scope = ChangeTrackingScope.Root)
    {
        var subscription = new Subscription<TValue>(State, selector, null, null, new ChangeTrackingScopeEqualityComparer<TValue>(scope));
        _watches.Add(subscription);
        return new Disposable(() => _watches.Remove(subscription));
    }

    public IDisposable Watch<TValue>(Func<TState, TValue> selector, Func<TValue, bool>? predicate, ChangeTrackingScope scope = ChangeTrackingScope.Root)
    {
        var subscription = new Subscription<TValue>(State, selector, predicate, null, new ChangeTrackingScopeEqualityComparer<TValue>(scope));
        _watches.Add(subscription);
        return new Disposable(() => _watches.Remove(subscription));
    }

    public IDisposable Watch<TValue>(Func<TState, TValue> selector, Func<TValue, bool>? predicate, Action<TValue?>? subscriber, ChangeTrackingScope scope = ChangeTrackingScope.Root)
    {
        var subscription = new Subscription<TValue>(State, selector, predicate, subscriber, new ChangeTrackingScopeEqualityComparer<TValue>(scope));
        _watches.Add(subscription);
        return new Disposable(() => _watches.Remove(subscription));
    }

    public IDisposable OnChange(Action subscriber) => OnChange(_ => subscriber());

    public IDisposable OnChange(Action<TState> subscriber)
    {
        _subscribers.Add(subscriber);
        return new Disposable(() => _subscribers.Remove(subscriber));
    }

    public void Dispose()
    {
        _subscribers.Clear();
        _watches.Clear();
        _disposeAction?.Invoke(this);
    }

    private sealed class Subscription<TValue> : IObserver<TState>, IChangeTracking
    {
        private readonly Func<TState, TValue> _selector;
        private readonly Func<TValue, bool>? _predicate;
        private readonly Action<TValue>? _subscriber;
        private readonly IEqualityComparer<TValue> _equalityComparer;
        private TValue? _value;
        private bool _changed;

        public Subscription(
            TState state,
            Func<TState, TValue> selector,
            Func<TValue, bool>? predicate,
            Action<TValue?>? subscriber,
            IEqualityComparer<TValue> equalityComparer)
        {
            _selector = selector;
            _predicate = predicate;
            _subscriber = subscriber;
            _equalityComparer = equalityComparer;
            SetValue(state);
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
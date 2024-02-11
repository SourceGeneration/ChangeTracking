using SourceGeneration.Rx;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Runtime.ExceptionServices;

namespace SourceGeneration.States;

public class State<TState> : IState<TState>, IStore<TState>
{
    private readonly BehaviorSubject<TState> _subject;
    private readonly BehaviorSubject<TState> _afterChange;
    private readonly WhereSubject<TState> _afterDistinctUtilChange;

    private readonly State<TState>? _parent;
    private readonly IDisposable? _parentSubscription;
    private ImmutableList<IChangeTracking> _bindings = ImmutableList.Create<IChangeTracking>();
    private ImmutableList<IDisposable> _afterUpdateSubscribes = ImmutableList.Create<IDisposable>();
    private TState _value;

    public State(TState state)
    {
        _value = state;
        _subject = new BehaviorSubject<TState>(_value);
        _afterChange = new BehaviorSubject<TState>(_value);
        _afterDistinctUtilChange = new WhereSubject<TState>(_afterChange, _ => HasBindingChanged());
        AcceptStateChanges();
    }

    internal State(State<TState> parent)
    {
        _value = parent.Value;
        _subject = new BehaviorSubject<TState>(_value);
        _parent = parent;
        _afterChange = parent._afterChange;
        _afterDistinctUtilChange = new WhereSubject<TState>(_afterChange, _ => HasBindingChanged());
        _parentSubscription = _parent.Subscribe(_subject);
    }

    public TState Value => _value;

    public void Update(Action<TState> action)
    {
        try
        {
            if (_parent == null)
            {
                action(_value);

                if (!_subject.IsDisposed)
                {
                    _subject.OnNext(_value);
                    _afterChange.OnNext(_value);
                }

                AcceptStateChanges();
            }
            else
            {
                _parent.Update(action);
            }

            AcceptBindingChanges();
        }
        catch (ObjectDisposedException) { }
        catch (Exception ex)
        {
            ExceptionDispatchInfo.Capture(ex).Throw();
        }
    }

    //public void Set(TState state)
    //{
    //    var newValue = ChangeTrackingProxyFactory.Create(state);
    //    _value = newValue;

    //    try
    //    {
    //        if (_parent == null)
    //        {

    //            if (!_subject.IsDisposed)
    //            {
    //                _subject.OnNext(_value);
    //                _afterChange.OnNext(_value);
    //            }
    //        }
    //        else
    //        {
    //            _parent.Set(_value);
    //        }

    //        AcceptBindingChanges();
    //        AcceptStateChanges();
    //    }
    //    catch (ObjectDisposedException) { }
    //    catch (Exception ex)
    //    {
    //        ExceptionDispatchInfo.Capture(ex).Throw();
    //    }
    //}

    private void AcceptStateChanges()
    {
        if (_value is IChangeTracking tacking)
        {
            tacking.AcceptChanges();
        }
    }

    private void AcceptBindingChanges()
    {
        var bindings = _bindings;
        foreach (var binding in bindings)
        {
            binding.AcceptChanges();
        }
    }

    public IDisposable Subscribe(IObserver<TState> observer) => _subject.Subscribe(observer);

    public IDisposable Bind<TValue>(Func<TState, TValue> selector, Action<TValue> subscriber, ChangeTrackingScope changeTrackingScope = ChangeTrackingScope.RootChanged)
    {
        var binding = new Binding<TValue>(
            observable: new SelectSubject<TState, TValue>(_subject, selector),
            subscriber: subscriber,
            disposeCallback: b => _bindings = _bindings.Remove(b),
            new ChangeTrackingScopeEqualityComparer<TValue>(changeTrackingScope));

        _bindings = _bindings.Add(binding);

        return binding;
    }

    public IDisposable Bind<TValue>(Func<TState, TValue> selector, Action<TValue> subscriber, IEqualityComparer<TValue> equalityComparer)
    {
        var binding = new Binding<TValue>(
            observable: new SelectSubject<TState, TValue>(_subject, selector),
            subscriber: subscriber,
            disposeCallback: b => _bindings = _bindings.Remove(b),
            equalityComparer);

        _bindings = _bindings.Add(binding);

        return binding;
    }


    private bool HasBindingChanged() => _bindings.Any(x => x.IsChanged);

    public IDisposable SubscribeBindingChanged(Action next) => SubscribeBindingChanged(_ => next());

    public IDisposable SubscribeBindingChanged(Action<TState> next)
    {
        var inner = _afterDistinctUtilChange.Subscribe(next);

        Disposable disposable = new(s =>
        {
            inner.Dispose();
            _afterUpdateSubscribes = _afterUpdateSubscribes.Remove(s);
        });
        _afterUpdateSubscribes = _afterUpdateSubscribes.Add(disposable);
        return inner;
    }

    public void Dispose()
    {
        _afterDistinctUtilChange.Dispose();
        _subject.Dispose();

        var bindings = _bindings;
        foreach (var binding in bindings)
        {
            ((IDisposable)binding).Dispose();
        }

        var subscribes = _afterUpdateSubscribes;
        foreach (var subscribe in subscribes)
        {
            subscribe.Dispose();
        }

        if (_parentSubscription == null)
        {
            _afterChange.Dispose();
        }
        else
        {
            _parentSubscription.Dispose();
        }

        GC.SuppressFinalize(this);
    }


    private sealed class Disposable(Action<Disposable> callback) : IDisposable
    {
        private bool _disposed;

        public void Dispose()
        {
            if (!_disposed)
            {
                callback(this);
                _disposed = true;
            }
        }
    }

    private sealed class Binding<TValue> : IObserver<TValue>, IChangeTracking, IDisposable
    {
        private readonly BehaviorSubject<TValue> _observable;
        private readonly Action<IChangeTracking> _disposeCallback;
        private readonly IDisposable _disposable;
        private readonly IEqualityComparer<TValue> _equalityComparer;
        private readonly Action<TValue> _subscriber;
        private bool _disposed;
        private bool _changed;
        private TValue _value = default!;

        public Binding(BehaviorSubject<TValue> observable, Action<TValue> subscriber, Action<IChangeTracking> disposeCallback, IEqualityComparer<TValue> equalityComparer)
        {
            _observable = observable;
            _subscriber = subscriber;
            _equalityComparer = equalityComparer;
            _disposeCallback = disposeCallback;

            _disposable = observable.Subscribe(this);
        }


        public bool IsChanged => _changed;

        public void AcceptChanges()
        {
            _changed = true;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _changed = false;
                _disposable.Dispose();
                _observable.Dispose();
                _disposeCallback(this);
                _disposed = true;
            }
        }

        public void OnCompleted() { }
        public void OnError(Exception error) { }

        public void OnNext(TValue value)
        {
            if (_disposed)
            {
                return;
            }

            if (!_equalityComparer.Equals(_value, value))
            {
                _value = value;
                _changed = true;
                _subscriber(_value);
            }
        }

    }
}
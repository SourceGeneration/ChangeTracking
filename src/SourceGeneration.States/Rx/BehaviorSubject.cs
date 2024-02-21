using System.Diagnostics.CodeAnalysis;
using System.Runtime.ExceptionServices;

namespace SourceGeneration.Rx;

internal class BehaviorSubject<T>(T value) : IObserver<T>, IObservable<T>, IDisposable
{
    private readonly object _gate = new();

    private T _value = value;
    private bool _isStopped;
    private Exception? _exception;
    private bool _isDisposed;
    internal ImmutableList _observers = ImmutableList.Empty;

    public bool HasObservers => _observers?.Data.Length > 0;

    public bool IsDisposed
    {
        get
        {
            lock (_gate)
            {
                return _isDisposed;
            }
        }
    }

    public T Value
    {
        get
        {
            lock (_gate)
            {
                CheckDisposed();

                if (_exception != null)
                {
                    ExceptionDispatchInfo.Capture(_exception).Throw();
                }

                return _value;
            }
        }
    }

    public bool TryGetValue([MaybeNullWhen(false)] out T value)
    {
        lock (_gate)
        {
            if (_isDisposed)
            {
                value = default;
                return false;
            }

            if (_exception != null)
            {
                ExceptionDispatchInfo.Capture(_exception).Throw();
            }

            value = _value;
            return true;
        }
    }

    public void OnCompleted()
    {
        IObserver<T>[]? os = null;

        lock (_gate)
        {
            CheckDisposed();

            if (!_isStopped)
            {
                os = _observers.Data;
                _observers = ImmutableList.Empty;
                _isStopped = true;
            }
        }

        if (os != null)
        {
            foreach (var o in os)
            {
                o.OnCompleted();
            }
        }
    }

    public void OnError(Exception error)
    {
        ArgumentNullException.ThrowIfNull(error);

        IObserver<T>[]? os = null;

        lock (_gate)
        {
            CheckDisposed();

            if (!_isStopped)
            {
                os = _observers.Data;
                _observers = ImmutableList.Empty;
                _isStopped = true;
                _exception = error;
            }
        }

        if (os != null)
        {
            foreach (var o in os)
            {
                o.OnError(error);
            }
        }
    }

    public void OnNext(T value)
    {
        IObserver<T>[]? os = null;

        lock (_gate)
        {
            CheckDisposed();

            if (!_isStopped)
            {
                _value = value;
                os = _observers.Data;
            }
        }

        if (os != null)
        {
            OnNext(value, os);
        }
    }

    protected virtual void OnNext(T value, params IObserver<T>[] observers)
    {
        foreach (var o in observers)
            o.OnNext(value);
    }

    public IDisposable Subscribe(IObserver<T> observer)
    {
        ArgumentNullException.ThrowIfNull(observer);

        Exception? ex;

        lock (_gate)
        {
            CheckDisposed();

            if (!_isStopped)
            {
                _observers = _observers.Add(observer);
                OnNext(_value, observer);
                return new Subscription(this, observer);
            }

            ex = _exception;
        }

        if (ex != null)
        {
            observer.OnError(ex);
        }
        else
        {
            observer.OnCompleted();
        }

        return Disposable.Empty;
    }

    private void Unsubscribe(IObserver<T> observer)
    {
        lock (_gate)
        {
            if (!_isDisposed)
            {
                _observers = _observers.Remove(observer);
            }
        }
    }

    public virtual void Dispose()
    {
        lock (_gate)
        {
            _isDisposed = true;
            _observers = null!; // NB: Disposed checks happen prior to accessing _observers.
            _value = default!;
            _exception = null;
        }
    }

    private void CheckDisposed()
    {
#if NET7_0_OR_GREATER
        ObjectDisposedException.ThrowIf(_isDisposed, this);
#else
        if (_isDisposed)
        {
            throw new ObjectDisposedException("BehaviorSubject");
        }
#endif
    }

    private sealed class Subscription(BehaviorSubject<T> subject, IObserver<T> observer) : IDisposable
    {
        private IObserver<T>? _observer = observer;

        public void Dispose()
        {
            var observer = Interlocked.Exchange(ref _observer, null);
            if (observer == null)
            {
                return;
            }

            subject.Unsubscribe(observer);
            subject = null!;
        }
    }


    private sealed class Disposable : IDisposable
    {
        public static readonly Disposable Empty = new();
        public void Dispose() { }
    }

    internal sealed class ImmutableList
    {
        public static readonly ImmutableList Empty = new();

        private readonly IObserver<T>[] _data;

        private ImmutableList() => _data = [];

        public ImmutableList(IObserver<T>[] data) => _data = data;

        public IObserver<T>[] Data => _data;

        public ImmutableList Add(IObserver<T> value)
        {
            var newData = new IObserver<T>[_data.Length + 1];

            Array.Copy(_data, newData, _data.Length);
            newData[_data.Length] = value;

            return new ImmutableList(newData);
        }

        public ImmutableList Remove(IObserver<T> value)
        {
            var i = Array.IndexOf(_data, value);
            if (i < 0)
            {
                return this;
            }

            var length = _data.Length;
            if (length == 1)
            {
                return Empty;
            }

            var newData = new IObserver<T>[length - 1];

            Array.Copy(_data, 0, newData, 0, i);
            Array.Copy(_data, i + 1, newData, i, length - i - 1);

            return new ImmutableList(newData);
        }
    }

}
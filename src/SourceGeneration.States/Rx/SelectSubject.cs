using System;

namespace SourceGeneration.Rx;

internal class SelectSubject<TSource, TResult> : BehaviorSubject<TResult>, IObserver<TSource>
{
    private IDisposable _subscription;
    private Func<TSource, TResult>? _selector;

    public SelectSubject(BehaviorSubject<TSource> source, Func<TSource, TResult> selector) : base(selector(source.Value))
    {
        _selector = selector;
        _subscription = source.Subscribe(this);
    }

    public void OnNext(TSource value)
    {
        if (!IsDisposed)
        {
            OnNext(_selector!(value));
        }
    }

    public override void Dispose()
    {
        if (!IsDisposed)
        {
            base.Dispose();
            _subscription.Dispose();
            _subscription = null!;
            _selector = null;
        }
    }
}


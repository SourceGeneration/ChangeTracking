namespace SourceGeneration.Rx;

internal class SelectSubject<TSource, TResult> : BehaviorSubject<TResult>
{
    private IDisposable _subscription;
    private Func<TSource, TResult>? _selector;

    public SelectSubject(BehaviorSubject<TSource> source, Func<TSource, TResult> selector) : base(selector(source.Value))
    {
        _selector = selector;
        _subscription = source.Subscribe(new AnonymousObserver<TSource>(
            next =>
            {
                if (!IsDisposed)
                {
                    OnNext(_selector(next));
                }
            },
            OnError,
            OnCompleted));
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


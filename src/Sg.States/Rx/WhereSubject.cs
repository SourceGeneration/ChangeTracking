namespace SourceGeneration.Rx;

internal class WhereSubject<T> : BehaviorSubject<T>, IDisposable
{
    private IDisposable _subscription;
    private Func<T, bool>? _predicate;

    public WhereSubject(BehaviorSubject<T> source, Func<T, bool> predicate) : base(source.Value)
    {
        _predicate = predicate;
        _subscription = source.Subscribe(new AnonymousObserver<T>(
            next =>
            {
                if (!IsDisposed && _predicate(next))
                    OnNext(next);
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
            _predicate = null;
        }
    }
}


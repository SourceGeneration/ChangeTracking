namespace SourceGeneration.Rx;

internal class WhereSubject<T> : BehaviorSubject<T>, IObserver<T>, IDisposable
{
    private IDisposable _subscription;
    private Func<T, bool>? _predicate;

    public WhereSubject(BehaviorSubject<T> source, Func<T, bool> predicate) : base(source.Value)
    {
        _predicate = predicate;
        _subscription = source.Subscribe(this);
    }

    protected override void OnNext(T value, IObserver<T>[] observers)
    {
        if (!IsDisposed && _predicate!(value))
        {
            base.OnNext(value, observers);
        }
    }

    public override void Dispose()
    {
        if (!IsDisposed)
        {
            _subscription.Dispose();
            _subscription = null!;
            _predicate = null;
            base.Dispose();
        }
    }
}


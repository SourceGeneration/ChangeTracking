namespace Sg.Rx;

internal sealed class AnonymousObserver<T>(Action<T> onNext, Action<Exception> onError, Action onCompleted) : IObserver<T>
{
    public void OnCompleted() => onCompleted();
    public void OnError(Exception error) => onError(error);
    public void OnNext(T value) => onNext(value);
}

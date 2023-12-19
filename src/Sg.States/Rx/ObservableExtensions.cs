using System.Runtime.ExceptionServices;

namespace SourceGeneration.Rx;

internal static partial class ObservableExtensions
{
    private static readonly Action<Exception> rethrow = e => ExceptionDispatchInfo.Capture(e).Throw();
    private static readonly Action nop = () => { };

    public static IDisposable Subscribe<T>(this IObservable<T> source, Action<T> onNext) => Subscribe(source, onNext, rethrow, nop);
    public static IDisposable Subscribe<T>(this IObservable<T> source, Action<T> onNext, Action<Exception> onError) => Subscribe(source, onNext, onError, nop);
    public static IDisposable Subscribe<T>(this IObservable<T> source, Action<T> onNext, Action onCompleted) => Subscribe(source, onNext, rethrow, onCompleted);
    public static IDisposable Subscribe<T>(this IObservable<T> source, Action<T> onNext, Action<Exception> onError, Action onCompleted) => source.Subscribe(new AnonymousObserver<T>(onNext, onError, onCompleted));
}

using System.Collections;

namespace SourceGeneration.States;

public static class IStateExtensions
{
    public static IDisposable BindSequence<TState, TValue>(this IState<TState> state, Func<TState, TValue> selector, Action<TValue> subscriber) where TValue : IEnumerable
    {
        return state.Bind(selector, subscriber, ChangeTrackingSequenceEqualityComparer<TValue>.Default);
    }
}

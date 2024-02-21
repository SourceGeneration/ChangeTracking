namespace SourceGeneration.States;

public interface IState : IDisposable
{
    bool IsRoot { get; }
    IDisposable SubscribeBindingChanged(Action next);
}

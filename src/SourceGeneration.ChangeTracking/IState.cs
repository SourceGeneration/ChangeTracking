namespace SourceGeneration.ChangeTracking;

public interface IState<TSelf> where TSelf : class, IState<TSelf>
{
    IChangeTracker<TSelf> CreateTracker();
    void AcceptChanges();
}

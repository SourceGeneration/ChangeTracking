using System.Collections.Immutable;
using System.ComponentModel;

namespace SourceGeneration.ChangeTracking;

public class State
{
    private ImmutableArray<IChangeTracker> _trackers = [];

    public IChangeTracker<TState> CreateTracker<TState>(TState state) where TState : class
    {
        var tracker = new ChangeTracker<TState>(state, x => _trackers = _trackers.Remove(x));
        _trackers = _trackers.Add(tracker);
        return tracker;
    }

    public void AcceptChanges()
    {
        var trackers = _trackers;
        foreach (var tracker in trackers)
        {
            tracker.AcceptChanges();
        }

        if (this is IChangeTracking tracking)
        {
            tracking.AcceptChanges();
        }
    }
}

public class State<TSelf> : State where TSelf : State<TSelf>
{
    public IChangeTracker<TSelf> CreateTracker() => CreateTracker((TSelf)this);
}


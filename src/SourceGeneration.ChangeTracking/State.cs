using System.Collections.Immutable;
using System.ComponentModel;

namespace SourceGeneration.ChangeTracking;

public class State<TSelf> where TSelf : State<TSelf>
{
    private ImmutableArray<ChangeTracker<TSelf>> _trackers = [];

    public IChangeTracker<TSelf> CreateTracker()
    {
        var tracker = new ChangeTracker<TSelf>((TSelf)this, x => _trackers = _trackers.Remove(x));
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
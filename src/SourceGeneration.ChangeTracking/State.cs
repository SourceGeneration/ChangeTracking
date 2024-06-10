using System.Collections.Generic;

namespace SourceGeneration.ChangeTracking;

public class State<TSelf> where TSelf : State<TSelf>
{
    private readonly List<ChangeTracker<TSelf>> _trackers = [];

    public IChangeTracker<TSelf> CreateTracker()
    {
        var tracker = new ChangeTracker<TSelf>((TSelf)this, x => _trackers.Remove(x));
        _trackers.Add(tracker);
        return tracker;
    }

    public void AcceptChanges()
    {
        foreach (var tracker in _trackers)
        {
            tracker.AcceptChanges();
        }
    }
}
using SourceGeneration.ChangeTracking.Test;

namespace SourceGeneration.ChangeTracking.Tracker;


[TestClass]
public partial class ChangeTrackerInstanceTest
{
    [TestMethod]
    public void ChangeReference()
    {
        var state = new TrackingObject();
        ChangeTracker<TrackingObject> tracker = new(state);

        using var disposable = tracker.Watch(x => x.CascadingObject, ChangeTrackingScope.Instance);

        int changes = 0;
        tracker.OnChange(() => changes++);

        state.CascadingObject = new();
        tracker.AcceptChanges();

        Assert.AreEqual(1, changes);

        state.CascadingObject.IntProperty = 2;
        tracker.AcceptChanges();

        Assert.AreEqual(1, changes);

    }

    [TestMethod]
    public void UnchangeReference()
    {
        var state = new TrackingObject
        {
            CascadingObject = new()
        };
        ChangeTracker<TrackingObject> tracker = new(state);

        using var disposable = tracker.Watch(x => x.CascadingObject, ChangeTrackingScope.Instance);

        int changes = 0;
        tracker.OnChange(() => changes++);

        state.CascadingObject.IntProperty = 2;
        tracker.AcceptChanges();

        Assert.AreEqual(0, changes);
    }

}

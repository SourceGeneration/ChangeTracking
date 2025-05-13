using SourceGeneration.ChangeTracking.Test;

namespace SourceGeneration.ChangeTracking.Tracker;

[TestClass]
public partial class ChangeTrackerInstancePropertyTest
{
    [TestMethod]
    public void ChangeReference()
    {
        var state = new TrackingObject();
        ChangeTracker<TrackingObject> tracker = new(state);

        using var disposable = tracker.Watch(x => x.CascadingObject, ChangeTrackingScope.InstanceProperty);

        int changes = 0;
        tracker.OnChange(() => changes++);

        state.CascadingObject = new();
        tracker.AcceptChanges();

        Assert.AreEqual(1, changes);

        state.CascadingObject.IntProperty = 2;
        tracker.AcceptChanges();

        Assert.AreEqual(2, changes);
    }

    [TestMethod]
    public void ChangeProperty()
    {
        var state = new TrackingObject
        {
            CascadingObject = new()
        };
        ChangeTracker<TrackingObject> tracker = new(state);

        using var disposable = tracker.Watch(x => x.CascadingObject, ChangeTrackingScope.InstanceProperty);

        int changes = 0;
        tracker.OnChange(() => changes++);

        state.CascadingObject.IntProperty = 2;
        tracker.AcceptChanges();

        Assert.AreEqual(1, changes);
    }

}

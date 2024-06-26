namespace SourceGeneration.ChangeTracking.Tracker;

[TestClass]
public class ChangeTrackerTest
{
    [TestMethod]
    public void Modify_Watch_Property()
    {
        var state = new TrackingTarget();
        ChangeTracker<TrackingTarget> tracker = new(state);

        bool changed = false;
        tracker.Watch(x => x.Value1);
        tracker.OnChange(() => changed = true);

        state.Value1 = 1;
        tracker.AcceptChanges();

        Assert.IsTrue(changed);
    }

    [TestMethod]
    public void Modify_Watch_Collection()
    {
        var state = new TrackingTarget();
        ChangeTracker<TrackingTarget> tracker = new(state);

        bool changed = false;
        tracker.Watch(x => x.List);
        tracker.OnChange(() => changed = true);

        state.List.Add(1);
        tracker.AcceptChanges();

        Assert.IsTrue(changed);
    }

    [TestMethod]
    public void Modify_NotWatch_Property()
    {
        var state = new TrackingTarget();
        ChangeTracker<TrackingTarget> tracker = new(state);

        bool changed = false;
        tracker.Watch(x => x.Value1);
        tracker.OnChange(() => changed = true);

        state.Value2 = "a";
        tracker.AcceptChanges();

        Assert.IsFalse(changed);
    }

    [TestMethod]
    public void Watch_Dispose()
    {
        var state = new TrackingTarget();
        ChangeTracker<TrackingTarget> tracker = new(state);
        bool changed = false;

        var disposable = tracker.Watch(x => x.Value1);
        tracker.OnChange(() => changed = true);

        state.Value1 = 1;
        tracker.AcceptChanges();

        Assert.IsTrue(changed);

        changed = false;
        disposable.Dispose();
        state.Value1 = 2;
        tracker.AcceptChanges();
        Assert.IsFalse(changed);
    }

    public class TrackingTarget
    {
        public int? Value1 { get; set; }
        public string? Value2 { get; set; }

        public ChangeTrackingList<int> List { get; set; } = [];
    }
}

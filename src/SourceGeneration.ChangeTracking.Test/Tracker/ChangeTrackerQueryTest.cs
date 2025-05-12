namespace SourceGeneration.ChangeTracking.Tracker;

[TestClass]
public partial class ChangeTrackerQueryTest
{
    [TestMethod]
    public void Add()
    {
        var state = new TrackingTarget();
        ChangeTracker<TrackingTarget> tracker = new(state);

        bool changed = false;
        var disposable = tracker.Watch<ChangeTrackingList<int>, int>(x => x.List, x => x > 5);

        tracker.OnChange(() => changed = true);
        state.List.Add(1);
        tracker.AcceptChanges();
        Assert.IsFalse(changed);

        state.List.Add(2);
        tracker.AcceptChanges();
        Assert.IsFalse(changed);

        state.List.Add(6);
        tracker.AcceptChanges();
        Assert.IsTrue(changed);

        changed = false;
        state.List.Add(5);
        tracker.AcceptChanges();
        Assert.IsFalse(changed);
    }

    [TestMethod]
    public void Remove()
    {
        var state = new TrackingTarget
        {
            List = [1, 2, 3, 4, 5, 6, 7, 8, 9]
        };

        ChangeTracker<TrackingTarget> tracker = new(state);

        bool changed = false;
        var disposable = tracker.Watch<ChangeTrackingList<int>, int>(x => x.List, x => x > 5);

        tracker.OnChange(() => changed = true);
        state.List.Remove(1);
        tracker.AcceptChanges();
        Assert.IsFalse(changed);

        state.List.Remove(2);
        tracker.AcceptChanges();
        Assert.IsFalse(changed);

        state.List.Remove(6);
        tracker.AcceptChanges();
        Assert.IsTrue(changed);

        changed = false;
        state.List.Remove(3);
        tracker.AcceptChanges();
        Assert.IsFalse(changed);
    }

    [TestMethod]
    public void Replace()
    {
        var state = new TrackingTarget
        {
            List = [1, 2, 3, 4, 5, 6, 7, 8, 9]
        };

        ChangeTracker<TrackingTarget> tracker = new(state);

        bool changed = false;
        var disposable = tracker.Watch<ChangeTrackingList<int>, int>(x => x.List, x => x > 5);

        tracker.OnChange(() => changed = true);
        state.List[0] = 0;
        tracker.AcceptChanges();
        Assert.IsFalse(changed);

        state.List[1] = 1;
        tracker.AcceptChanges();
        Assert.IsFalse(changed);

        state.List[2] = 12;
        tracker.AcceptChanges();
        Assert.IsTrue(changed);

        changed = false;
        state.List[8] = 1;
        tracker.AcceptChanges();
        Assert.IsTrue(changed);
    }

    [TestMethod]
    public void Dispose()
    {
        var state = new TrackingTarget();
        ChangeTracker<TrackingTarget> tracker = new(state);

        bool changed = false;
        var disposable = tracker.Watch<ChangeTrackingList<int>, int>(x => x.List, x => x > 5);

        tracker.OnChange(() => changed = true);

        state.List.Add(6);
        tracker.AcceptChanges();
        Assert.IsTrue(changed);

        disposable.Dispose();

        changed = false;
        state.List.Add(7);
        tracker.AcceptChanges();
        Assert.IsFalse(changed);

    }

    [TestMethod]
    public void Watch_More()
    {
        var state = new TrackingTarget();
        ChangeTracker<TrackingTarget> tracker = new(state);

        int changes = 0;
        var disposable1 = tracker.Watch<ChangeTrackingList<int>, int>(x => x.List, x => x > 5);
        var disposable2 = tracker.Watch<ChangeTrackingList<int>, int>(x => x.List, x => x > 10);

        tracker.OnChange(() => changes++);

        state.List.Add(6);
        tracker.AcceptChanges();
        Assert.AreEqual(1, changes);

        state.List.Add(7);
        tracker.AcceptChanges();
        Assert.AreEqual(2, changes);

        state.List.Add(11);
        tracker.AcceptChanges();
        Assert.AreEqual(3, changes);

        state.List.Add(5);
        tracker.AcceptChanges();
        Assert.AreEqual(3, changes);

        disposable1.Dispose();
        state.List.Add(8);
        tracker.AcceptChanges();
        Assert.AreEqual(3, changes);

    }

    [TestMethod]
    public void Scope_Root()
    {
        var state = new TrackingTarget();
        ChangeTracker<TrackingTarget> tracker = new(state);

        bool changed = false;
        var disposable1 = tracker.Watch<ChangeTrackingList<TrackingObject>, TrackingObject>(x => x.ObjectList, x => x.Value > 5, scope: ChangeTrackingScope.Root);

        tracker.OnChange(() => changed = true);

        state.ObjectList.Add(new TrackingObject
        {
            Value = 1,
        });
        tracker.AcceptChanges();
        Assert.IsFalse(changed);

        state.ObjectList.Add(new TrackingObject
        {
            Value = 6,
        });
        tracker.AcceptChanges();
        Assert.IsTrue(changed);

        changed = false;
        state.ObjectList[0].Text = "a";
        tracker.AcceptChanges();
        Assert.IsFalse(changed);

        changed = false;
        state.ObjectList[1].Text = "a";
        tracker.AcceptChanges();
        Assert.IsTrue(changed);

    }

    [TestMethod]
    public void Scope_Cascading()
    {
        var state = new TrackingTarget();
        ChangeTracker<TrackingTarget> tracker = new(state);

        int changes = 0;
        var disposable1 = tracker.Watch<ChangeTrackingList<TrackingObject>, TrackingObject>(x => x.ObjectList, x => x.Value > 5, scope: ChangeTrackingScope.Cascading);

        tracker.OnChange(() => changes++);

        state.ObjectList.Add(new TrackingObject
        {
            Value = 1,
        });
        tracker.AcceptChanges();
        Assert.AreEqual(0, changes);

        state.ObjectList.Add(new TrackingObject
        {
            Value = 6,
        });
        tracker.AcceptChanges();
        Assert.AreEqual(1, changes);

        state.ObjectList[1].Text = "a";
        tracker.AcceptChanges();
        Assert.AreEqual(2, changes);

    }

    [TestMethod]
    public void ChangeInstance()
    {
        var state = new TrackingTarget();
        ChangeTracker<TrackingTarget> tracker = new(state);

        int changes = 0;
        var disposable = tracker.Watch<ChangeTrackingList<int>, int>(x => x.List, x => x > 5);

        tracker.OnChange(() => changes++);
        state.List.Add(1);
        tracker.AcceptChanges();
        Assert.AreEqual(0, changes);

        state.List.Add(6);
        tracker.AcceptChanges();
        Assert.AreEqual(1, changes);

        state.List = [];

        state.List.Add(1);
        tracker.AcceptChanges();
        Assert.AreEqual(1, changes);

        state.List.Add(8);
        tracker.AcceptChanges();
        Assert.AreEqual(2, changes);
    }


    public class TrackingTarget
    {
        public ChangeTrackingList<int> List { get; set; } = [];

        public ChangeTrackingList<TrackingObject> ObjectList { get; set; } = [];
    }

    [ChangeTracking]
    public partial class TrackingObject
    {
        public partial int Value { get; set; }
        public partial string? Text { get; set; }
    }

}

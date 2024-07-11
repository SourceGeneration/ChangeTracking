namespace SourceGeneration.ChangeTracking.State;

[TestClass]
public class CascadingTrackingTest
{
    [TestMethod]
    public void Root_Value_Change()
    {
        var state = new CascadingTestState();

        bool changed = false;

        var tracker = state.CreateTracker();
        tracker.Watch(x => x, ChangeTrackingScope.Root);
        tracker.OnChange(() => changed = true);

        state.Object.Value = 1;
        state.AcceptChanges();

        Assert.IsFalse(changed);
    }

    [TestMethod]
    public void Cascading_Value_Change()
    {
        var state = new CascadingTestState();

        bool changed = false;

        var tracker = state.CreateTracker();
        tracker.Watch(x => x, ChangeTrackingScope.Cascading);
        tracker.OnChange(() => changed = true);

        state.Object.Value = 1;
        state.AcceptChanges();

        Assert.IsTrue(changed);
    }

    [TestMethod]
    public void Cascading_Value_NotChange()
    {
        var state = new CascadingTestState();

        bool changed = false;

        var tracker = state.CreateTracker();
        tracker.Watch(x => x, ChangeTrackingScope.Cascading);
        tracker.OnChange(() => changed = true);

        state.Object.Value = 0;
        state.AcceptChanges();

        Assert.IsFalse(changed);
    }

    [TestMethod]
    public void Root_Collection_Add()
    {
        var state = new CascadingTestState();

        bool changed = false;

        var tracker = state.CreateTracker();
        tracker.Watch(x => x.Object, ChangeTrackingScope.Root);
        tracker.OnChange(() => changed = true);

        state.Object.List.Add(1);
        state.AcceptChanges();

        Assert.IsFalse(changed);
    }

    [TestMethod]
    public void Root_Collection_Set()
    {
        var state = new CascadingTestState();

        bool changed = false;

        var tracker = state.CreateTracker();
        tracker.Watch(x => x.Object, ChangeTrackingScope.Root);
        tracker.OnChange(() => changed = true);

        state.Object.List[0] = 1;
        state.AcceptChanges();

        Assert.IsFalse(changed);
    }

    [TestMethod]
    public void Root_Collection_Remove()
    {
        var state = new CascadingTestState();

        bool changed = false;

        var tracker = state.CreateTracker();
        tracker.Watch(x => x.Object, ChangeTrackingScope.Root);
        tracker.OnChange(() => changed = true);

        state.Object.List.Remove(0);
        state.AcceptChanges();

        Assert.IsFalse(changed);
    }

    [TestMethod]
    public void Cascading_Collection_Add()
    {
        var state = new CascadingTestState();

        bool changed = false;

        var tracker = state.CreateTracker();
        tracker.Watch(x => x.Object, ChangeTrackingScope.Cascading);
        tracker.OnChange(() => changed = true);

        state.Object.List.Add(1);
        state.AcceptChanges();

        Assert.IsTrue(changed);
    }

    [TestMethod]
    public void Cascading_Collection_Remove()
    {
        var state = new CascadingTestState();

        bool changed = false;

        var tracker = state.CreateTracker();
        tracker.Watch(x => x.Object, ChangeTrackingScope.Cascading);
        tracker.OnChange(() => changed = true);

        state.Object.List.Remove(0);
        state.AcceptChanges();

        Assert.IsTrue(changed);
    }

    [TestMethod]
    public void Cascading_Collection_Set()
    {
        var state = new CascadingTestState();

        bool changed = false;

        var tracker = state.CreateTracker();
        tracker.Watch(x => x.Object, ChangeTrackingScope.Cascading);
        tracker.OnChange(() => changed = true);

        state.Object.List[0] = 1;
        state.AcceptChanges();

        Assert.IsTrue(changed);
    }


    [TestMethod]
    public void Cascading_Collection_Set_NotChange()
    {
        var state = new CascadingTestState();

        bool changed = false;

        var tracker = state.CreateTracker();
        tracker.Watch(x => x.Object, ChangeTrackingScope.Cascading);
        tracker.OnChange(() => changed = true);

        state.Object.List[0] = 0;
        state.AcceptChanges();

        Assert.IsFalse(changed);
    }

    [TestMethod]
    public void Cascading_ObjectCollection_Add()
    {
        var state = new CascadingTestState();

        bool changed = false;
        int subscribe = 0;

        var tracker = state.CreateTracker();
        tracker.OnChange(() => changed = true);
        tracker.Watch(x => x.List, _ => subscribe++, ChangeTrackingScope.Cascading);

        state.List.Add(new CascadingCollectionTestObject());
        state.AcceptChanges();

        Assert.IsTrue(changed);
        Assert.AreEqual(2, subscribe);

        changed = false;
        state.List[0].Value = 2;
        state.AcceptChanges();
        Assert.IsTrue(changed);
        Assert.AreEqual(3, subscribe);
    }
}

[ChangeTracking]
public partial class CascadingTestState : State<CascadingTestState>
{
    public CascadingTestState()
    {
        Object = new();
        List = [];
        __AcceptChanges();
    }

    public partial CascadingCollectionTestObject Object { get; set; }

    public partial ChangeTrackingList<CascadingCollectionTestObject> List { get; set; }
}

[ChangeTracking]
public partial class CascadingCollectionTestObject
{
    public CascadingCollectionTestObject()
    {
        List = [0];
    }

    public partial int Value { get; set; }
    public partial ChangeTrackingList<int> List { get; set; } 
}


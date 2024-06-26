namespace SourceGeneration.ChangeTracking.State;

[TestClass]
public class CascadingTrackingTest
{
    [TestMethod]
    public void Root_Value_Change()
    {
        var state = ChangeTrackingProxyFactory.Create(new CascadingTestState());

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
        var state = ChangeTrackingProxyFactory.Create(new CascadingTestState());

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
        var state = ChangeTrackingProxyFactory.Create(new CascadingTestState());

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
        var state = ChangeTrackingProxyFactory.Create(new CascadingTestState());

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
        var state = ChangeTrackingProxyFactory.Create(new CascadingTestState());

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
        var state = ChangeTrackingProxyFactory.Create(new CascadingTestState());

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
        var state = ChangeTrackingProxyFactory.Create(new CascadingTestState());

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
        var state = ChangeTrackingProxyFactory.Create(new CascadingTestState());

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
        var state = ChangeTrackingProxyFactory.Create(new CascadingTestState());

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
        var state = ChangeTrackingProxyFactory.Create(new CascadingTestState());

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
        var state = ChangeTrackingProxyFactory.Create(new CascadingTestState());

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
public class CascadingTestState : State<CascadingTestState>
{
    public virtual CascadingCollectionTestObject Object { get; set; } = new();

    public virtual ChangeTrackingList<CascadingCollectionTestObject> List { get; set; } = [];
}

[ChangeTracking]
public class CascadingCollectionTestObject
{
    public virtual int Value { get; set; }
    public virtual ChangeTrackingList<int> List { get; set; } = [0];
}


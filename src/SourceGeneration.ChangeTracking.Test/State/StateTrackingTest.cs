using System.Diagnostics;
using System.Threading.Channels;

namespace SourceGeneration.ChangeTracking.State;

[TestClass]
public class StateTrackingTest
{
    [TestMethod]
    public void Modify_Watch_Property()
    {
        var state = new TestState();
        bool changed = false;

        var tracker = state.CreateTracker();
        tracker.Watch(x => x.Value1);
        tracker.OnChange(() => changed = true);

        state.Value1 = 1;
        state.AcceptChanges();

        Assert.IsTrue(changed);
    }

    [TestMethod]
    public void Modify_Watch_Property_Unchanged()
    {
        var state = new TestState();
        bool changed = false;

        var tracker = state.CreateTracker();
        tracker.Watch(x => x.Value1);
        tracker.OnChange(() => changed = true);

        state.Value1 = 0;
        state.AcceptChanges();

        Assert.IsFalse(changed);
    }

    [TestMethod]
    public void Modify_NotWatch_Property()
    {
        var state = new TestState();
        bool changed = false;

        var tracker = state.CreateTracker();
        tracker.Watch(x => x.Value1);
        tracker.OnChange(() => changed = true);

        state.Value2 = "abcd";
        state.AcceptChanges();

        Assert.IsFalse(changed);
    }


    [TestMethod]
    public void Mulit_Tracker()
    {
        var state = new TestState();
        bool changed1 = false;
        bool changed2 = false;

        var tracker1 = state.CreateTracker();
        tracker1.Watch(x => x.Value1);
        tracker1.OnChange(() => changed1 = true);

        var tracker2 = state.CreateTracker();
        tracker2.Watch(x => x.Value2);
        tracker2.OnChange(() => changed2 = true);

        state.Value1 = 1;
        state.AcceptChanges();

        Assert.IsTrue(changed1);
        Assert.IsFalse(changed2);

        changed1 = false;
        state.Value2 = "a";
        state.AcceptChanges();
        Assert.IsFalse(changed1);
        Assert.IsTrue(changed2);

        changed1 = false;
        changed2 = false;
        state.Value1 = 2;
        state.Value2 = "b";
        state.AcceptChanges();
        Assert.IsTrue(changed1);
        Assert.IsTrue(changed2);
    }

    [TestMethod]
    public void Tracker_Dispose()
    {
        var state = new TestState();
        bool changed = false;

        var tracker = state.CreateTracker();
        tracker.Watch(x => x.Value1);
        tracker.OnChange(() => changed = true);

        state.Value1 = 1;
        state.AcceptChanges();

        Assert.IsTrue(changed);

        changed = false;
        tracker.Dispose();
        state.Value1 = 2;
        state.AcceptChanges();
        Assert.IsFalse(changed);
    }


    public class TestState : State<TestState>
    {
        public int Value1 { get; set; }
        public string? Value2 { get; set; }
    }
}


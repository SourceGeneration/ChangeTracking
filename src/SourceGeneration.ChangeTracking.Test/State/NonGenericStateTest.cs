namespace SourceGeneration.ChangeTracking.StateTest;

[TestClass]
public class NonGenericStateTest
{
    [TestMethod]
    public void TestChanged()
    {
        bool changed = false;

        NonGenericState1 state = new();
        var tracker = state.CreateTracker(state);
        tracker.Watch(x => x.A);
        tracker.OnChange(() => changed = true);
        state.A = 1;

        state.AcceptChanges();
        Assert.IsTrue(changed);
    }

    [TestMethod]
    public void TestNotChanged()
    {
        bool changed = false;

        NonGenericState1 state = new();
        var tracker = state.CreateTracker(state);
        tracker.Watch(x => x.A);
        tracker.OnChange(() => changed = true);
        state.B = 1;

        state.AcceptChanges();
        Assert.IsFalse(changed);
    }

    [TestMethod]
    public void MulitTracker()
    {
        bool changed1 = false;
        bool changed2 = false;

        State state = new();

        NonGenericState1 state1 = new();
        NonGenericState2 state2 = new();
        var tracker1 = state.CreateTracker(state1);
        tracker1.Watch(x => x.A);
        tracker1.OnChange(() => changed1 = true);

        var tracker2 = state.CreateTracker(state2);
        tracker2.Watch(x => x.B);
        tracker2.OnChange(() => changed2 = true);

        state1.B = 1;
        state2.B = 1;

        state.AcceptChanges();
        Assert.IsFalse(changed1);
        Assert.IsTrue(changed2);
    }

}


public class NonGenericState1 : State
{
    public int A { get; set; }
    public int B { get; set; }
}

public class NonGenericState2 : State
{
    public int A { get; set; }
    public int B { get; set; }
}

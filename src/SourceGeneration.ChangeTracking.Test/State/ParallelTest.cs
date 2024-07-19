namespace SourceGeneration.ChangeTracking.State;

[TestClass]
public partial class ParallelTest
{
    [TestMethod]
    public void Parallel_AcceptChanges()
    {
        var state = new TestState();
        bool changed = false;

        int sub1 = 0;
        int sub2 = 0;
        var tracker = state.CreateTracker();
        tracker.OnChange(() => changed = true);
        tracker.Watch(x => x.Value1, x => sub1++);
        tracker.Watch(x => x.Value2, x => sub2++);

        Assert.AreEqual(1, sub1);
        Assert.AreEqual(1, sub2);

        state.Value1 = 1;
        state.Value2 = 2;
        Parallel.For(0, 20, i =>
        {
            state.AcceptChanges();
        });

        Assert.AreEqual(1, sub1);
        Assert.AreEqual(2, sub2);
        Assert.IsTrue(changed);
    }

    [ChangeTracking]

    public partial class TestState : State<TestState>
    {
        public TestState()
        {
            Value1 = 1;
            Value2 = 1;
        }
        public partial int Value1 { get; set; }
        public partial int Value2 { get; set; }
    }
}


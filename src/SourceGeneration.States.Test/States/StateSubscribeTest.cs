namespace SourceGeneration.States.Test.States;

[TestClass]
public class StateSubscribeTest
{
    [TestMethod]
    public void BindTest()
    {
        State<TestState> state = new(new TestState());

        int count1 = 0;
        int count2 = 0;
        int changes = 0;

        state.Bind(x => x.Value1, x => count1++);
        state.Bind(x => x.Value2, x => count2++);

        state.SubscribeBindingChanged(() => changes++);

        Assert.AreEqual(1, count1);
        Assert.AreEqual(1, count2);
        Assert.AreEqual(1, changes);

        state.Update(x => x.Value1++);
        Assert.AreEqual(2, count1);
        Assert.AreEqual(1, count2);
        Assert.AreEqual(2, changes);

        state.Update(x => x.Value2++);
        Assert.AreEqual(2, count1);
        Assert.AreEqual(2, count2);
        Assert.AreEqual(3, changes);

        state.Update(x =>
        {
            x.Value1++;
            x.Value2++;
        });
        Assert.AreEqual(3, count1);
        Assert.AreEqual(3, count2);
        Assert.AreEqual(4, changes);
    }

    [TestMethod]
    public void UnchangeTest()
    {
        State<TestState> state = new(new TestState());

        int count1 = 0;
        int count2 = 0;
        int changes = 0;

        state.Bind(x => x.Value1, x => count1++);
        state.Bind(x => x.Value2, x => count2++);

        state.SubscribeBindingChanged(() => changes++);

        Assert.AreEqual(1, count1);
        Assert.AreEqual(1, count2);
        Assert.AreEqual(1, changes);

        state.Update(x => x.Value1 = 1);
        Assert.AreEqual(1, count1);
        Assert.AreEqual(1, count2);
        Assert.AreEqual(1, changes);

        state.Update(x => x.Value2 = 2);
        Assert.AreEqual(1, count1);
        Assert.AreEqual(1, count2);
        Assert.AreEqual(1, changes);

        state.Update(x => x.Value2 = 3);
        Assert.AreEqual(1, count1);
        Assert.AreEqual(2, count2);
        Assert.AreEqual(2, changes);
    }

    [TestMethod]
    public void TransientTest()
    {
        int valueFromTransient = 0;
        int valueFromRoot = 0;

        int changesFromTransient = 0;
        int changesFromRoot = 0;

        State<TestState> state = new(new TestState());

        var transient = state.CreateScope();

        state.Bind(x => x.Value1, x => valueFromRoot = x);
        state.SubscribeBindingChanged(() => changesFromRoot++);

        transient.Bind(x => x.Value1, x => valueFromTransient = x);
        transient.SubscribeBindingChanged(() => changesFromTransient++);

        Assert.AreEqual(1, valueFromRoot);
        Assert.AreEqual(1, valueFromTransient);
        Assert.AreEqual(1, changesFromRoot);
        Assert.AreEqual(1, changesFromTransient);

        state.Update(x => x.Value1++);

        Assert.AreEqual(2, valueFromRoot);
        Assert.AreEqual(2, valueFromTransient);
        Assert.AreEqual(2, changesFromRoot);
        Assert.AreEqual(2, changesFromTransient);

        transient.Dispose();
        state.Update(x => x.Value1++);

        Assert.AreEqual(3, valueFromRoot);
        Assert.AreEqual(2, valueFromTransient);
        Assert.AreEqual(3, changesFromRoot);
        Assert.AreEqual(2, changesFromTransient);
    }

    public class TestState
    {
        public int Value1 { get; set; } = 1;
        public int Value2 { get; set; } = 2;
    }
}

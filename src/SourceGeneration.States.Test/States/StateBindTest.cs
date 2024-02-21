namespace SourceGeneration.States.Test.States;

[TestClass]
public class StateBindTest
{
    [TestMethod]
    public void BindTest1()
    {
        State<TestState> state = new(new TestState());

        int value = 0;

        var binding = state.Bind(x => x.Value1, x => value = x);
        Assert.AreEqual(1, value);

        state.Update(x => x.Value1++);
        Assert.AreEqual(2, value);

        binding.Dispose();
        state.Update(x => x.Value1++);
        Assert.AreEqual(2, value);
    }

    [TestMethod]
    public void UnchangeTest()
    {
        State<TestState> state = new(new TestState
        {
            Value2 = 3,
            Value1 = 3,
        });

        int count1 = 0;
        int count2 = 0;
        state.Bind(x => x.Value1, x => count1++);
        state.Bind(x => x.Value2, x => count2++);

        Assert.AreEqual(1, count1);
        Assert.AreEqual(1, count2);

        state.Update(x => x.Value1 = 3);
        Assert.AreEqual(1, count1);
        Assert.AreEqual(1, count2);

        state.Update(x => x.Value2 = 3);
        Assert.AreEqual(1, count1);
        Assert.AreEqual(1, count2);

    }

    [TestMethod]
    public void BindTest2()
    {
        State<TestState> state = new(new TestState());

        int count1 = 0;
        int count2 = 0;

        var binding1 = state.Bind(x => x.Value1, x => count1++);
        var binding2 = state.Bind(x => x.Value2, x => count2++);

        Assert.AreEqual(1, count1);
        Assert.AreEqual(1, count2);

        state.Update(x => x.Value1++);
        Assert.AreEqual(2, count1);
        Assert.AreEqual(1, count2);


        state.Update(x => x.Value2++);
        Assert.AreEqual(2, count1);
        Assert.AreEqual(2, count2);

        state.Update(x =>
        {
            x.Value1++;
            x.Value2++;
        });
        Assert.AreEqual(3, count1);
        Assert.AreEqual(3, count2);

        binding1.Dispose();

        state.Update(x =>
        {
            x.Value1++;
            x.Value2++;
        });
        Assert.AreEqual(3, count1);
        Assert.AreEqual(4, count2);

        binding2.Dispose();
        state.Update(x =>
        {
            x.Value1++;
            x.Value2++;
        });
        Assert.AreEqual(3, count1);
        Assert.AreEqual(4, count2);

    }

    [TestMethod]
    public void TransientBindTest()
    {
        int valueFromTransient = 0;
        int valueFromRoot = 0;

        State<TestState> state = new(new TestState());

        var transient = state.CreateScope();

        state.Bind(x => x.Value1, x => valueFromRoot = x);
        transient.Bind(x => x.Value1, x => valueFromTransient = x);

        Assert.AreEqual(1, valueFromRoot);
        Assert.AreEqual(1, valueFromTransient);

        state.Update(x => x.Value1++);

        Assert.AreEqual(2, valueFromRoot);
        Assert.AreEqual(2, valueFromTransient);

        transient.Dispose();
        state.Update(x => x.Value1++);

        Assert.AreEqual(3, valueFromRoot);
        Assert.AreEqual(2, valueFromTransient);
    }

    public class TestState
    {
        public int Value1 { get; set; } = 1;
        public int Value2 { get; set; } = 2;
    }
}

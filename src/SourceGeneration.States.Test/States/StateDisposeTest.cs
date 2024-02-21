namespace SourceGeneration.States.Test.States;

[TestClass]
public class StateDisposeTest
{
    [TestMethod]
    public void DisposeBinding()
    {
        State<int> state = new(1);

        var disposable1 = state.Bind(x => x, x => { });
        var disposable2 = state.Bind(x => x, x => { });
        Assert.AreEqual(2, state._bindings.Count);

        disposable1.Dispose();
        Assert.AreEqual(1, state._bindings.Count);

        disposable2.Dispose();
        Assert.AreEqual(0, state._bindings.Count);
    }

    [TestMethod]
    public void DisposeSubscription()
    {
        State<int> state = new(1);

        var disposable1 = state.SubscribeBindingChanged(() => { });
        var disposable2 = state.SubscribeBindingChanged(() => { });
        Assert.AreEqual(2, state._subscriptions.Count);

        disposable1.Dispose();
        Assert.AreEqual(1, state._subscriptions.Count);

        disposable2.Dispose();
        Assert.AreEqual(0, state._subscriptions.Count);
    }

    [TestMethod]
    public void DisposeState()
    {
        State<int> state = new(1);
        state.Bind(x => x, x => { });
        state.Bind(x => x, x => { });
        state.SubscribeBindingChanged(() => { });
        state.SubscribeBindingChanged(() => { });

        Assert.AreEqual(2, state._bindings.Count);
        Assert.AreEqual(2, state._subscriptions.Count);

        state.Dispose();

        Assert.AreEqual(0, state._bindings.Count);
        Assert.AreEqual(0, state._subscriptions.Count);
    }

}

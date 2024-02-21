namespace SourceGeneration.States.Test.States;


[TestClass]
public class StateScopeTest
{
    [TestMethod]
    public void Transient()
    {
        State<int> state = new(1);

        using (var scope1 = state.CreateScope())
        {
            Assert.AreEqual(0, ((State<int>)scope1)._subject._observers.Data.Length);
            Assert.AreEqual(1, state._subject._observers.Data.Length);

            using (var scope2 = scope1.CreateScope())
            {
                Assert.AreEqual(0, ((State<int>)scope2)._subject._observers.Data.Length);
                Assert.AreEqual(1, ((State<int>)scope1)._subject._observers.Data.Length);
                Assert.AreEqual(1, state._subject._observers.Data.Length);
            }

            Assert.AreEqual(0, ((State<int>)scope1)._subject._observers.Data.Length);
        }
        Assert.AreEqual(0, state._subject._observers.Data.Length);
    }

}

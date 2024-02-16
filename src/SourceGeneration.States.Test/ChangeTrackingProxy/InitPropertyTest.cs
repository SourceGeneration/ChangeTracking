namespace SourceGeneration.States.Test.ChangeTracking;

[TestClass]
public class InitPropertyTest
{
    [TestMethod]
    public void TrackingObjectInit()
    {
        var tracking = ChangeTrackingProxyFactory.Create(new InitPropertyTestObject()
        {
            InitProperty = 1,
            RequiredInitProperty = 2,
            NestedProperty = new InitPropertyTestObject
            {
                InitProperty = 1,
                RequiredInitProperty = 2,
            }
        });

        Assert.AreEqual(1, tracking.InitProperty);
        Assert.AreEqual(2, tracking.RequiredInitProperty);
        Assert.AreEqual(1, tracking.NestedProperty!.InitProperty);
        Assert.AreEqual(2, tracking.NestedProperty!.RequiredInitProperty);
    }
}



[ChangeTracking]
public class InitPropertyTestObject
{
    public virtual int InitProperty { get; init; }
    public virtual required int RequiredInitProperty { get; init; }

    public virtual InitPropertyTestObject? NestedProperty { get; init; }
}
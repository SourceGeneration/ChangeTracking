namespace SourceGeneration.ChangeTracking.TrackObjects;

[TestClass]
public class InitPropertyTest
{
    [TestMethod]
    public void TrackingObjectInit()
    {
        var tracking = new InitPropertyTestObject()
        {
            InitProperty = 1,
            RequiredInitProperty = 2,
            NestedProperty = new InitPropertyTestObject
            {
                InitProperty = 1,
                RequiredInitProperty = 2,
            }
        };

        Assert.AreEqual(1, tracking.InitProperty);
        Assert.AreEqual(2, tracking.RequiredInitProperty);
        Assert.AreEqual(1, tracking.NestedProperty!.InitProperty);
        Assert.AreEqual(2, tracking.NestedProperty!.RequiredInitProperty);
    }
}



[ChangeTracking]
public partial class InitPropertyTestObject
{
    public partial int InitProperty { get; init; }
    public required partial int RequiredInitProperty { get; init; }

    public partial InitPropertyTestObject? NestedProperty { get; init; }
}
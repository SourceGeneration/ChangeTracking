namespace SourceGeneration.ChangeTracking.Test;

[TestClass]
public class RequredPropertyTest
{

    [TestMethod]
    public void RequirePropertyTrackingObject()
    {
        var value = new RequredPropertyTestObject
        {
            Array = [],
            Value = 1,
            Enumerable = []
        };
        var tracking = ChangeTrackingProxyFactory.Create(value);
        Assert.IsInstanceOfType<int[]>(tracking.Array);
        Assert.IsInstanceOfType<ChangeTrackingList<int>>(tracking.Enumerable);
    }
}

[ChangeTracking]
public class RequredPropertyTestObject
{
    public virtual int IntProperty { get; set; }
    public required virtual int Value { get; set; }
    public required virtual int[] Array { get; set; } = [];
    public required virtual IEnumerable<int> Enumerable { get; set; } = [];
}
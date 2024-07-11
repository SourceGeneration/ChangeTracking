namespace SourceGeneration.ChangeTracking.TrackObjects;

[TestClass]
public class RequredPropertyTest
{

    [TestMethod]
    public void RequirePropertyTrackingObject()
    {
        var tracking = new RequredPropertyTestObject
        {
            Array = [],
            Value = 1,
            Enumerable = []
        };
        Assert.IsInstanceOfType<int[]>(tracking.Array);
        Assert.IsInstanceOfType<ChangeTrackingList<int>>(tracking.Enumerable);
    }
}

[ChangeTracking]
public partial class RequredPropertyTestObject
{
    public partial int IntProperty { get; set; }
    public required partial int Value { get; set; }
    public required partial int[] Array { get; set; }
    public required partial IEnumerable<int> Enumerable { get; set; }
}
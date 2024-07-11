namespace SourceGeneration.ChangeTracking.TrackObjects;

[TestClass]
public class ReadOnlyPropertyTest
{
    [TestMethod]
    public void ReadOnlyPropertyObjectInit()
    {
        var tracking = new ReadOnlyPropertyTestObject();

        Assert.AreEqual(1, tracking.ReadOnlyProperty);
    }

    [TestMethod]
    public void ReadOnlyVirtualPropertyObjectInit()
    {
        var tracking = new ReadOnlyPropertyTestObject();

        Assert.AreEqual(1, tracking.ReadOnlyVirtualProperty);
    }
}

[ChangeTracking]
public partial class ReadOnlyPropertyTestObject
{
    public int ReadOnlyProperty { get; } = 1;
    public virtual int ReadOnlyVirtualProperty { get; } = 1;

    public partial int IntProperty { get; set; }

}

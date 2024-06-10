namespace SourceGeneration.ChangeTracking.Test;

[TestClass]
public class ReadOnlyPropertyTest
{
    [TestMethod]
    public void ReadOnlyPropertyObjectInit()
    {
        var value = new ReadOnlyPropertyTestObject();
        var tracking = ChangeTrackingProxyFactory.Create(value);

        Assert.AreEqual(1, tracking.ReadOnlyProperty);
    }

    [TestMethod]
    public void ReadOnlyVirtualPropertyObjectInit()
    {
        var value = new ReadOnlyPropertyTestObject();
        var tracking = ChangeTrackingProxyFactory.Create(value);

        Assert.AreEqual(1, tracking.ReadOnlyVirtualProperty);
    }
}

[ChangeTracking]
public class ReadOnlyPropertyTestObject
{
    public int ReadOnlyProperty { get; } = 1;
    public virtual int ReadOnlyVirtualProperty { get; } = 1;

    public virtual int IntProperty { get; set; }

}

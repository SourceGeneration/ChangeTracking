using static SourceGeneration.States.Test.ChangeTracking.InterfaceEnumerableChangeTrackingTest;

namespace SourceGeneration.States.Test.ChangeTracking;

[TestClass]
public class ProxyInitTest
{
    [TestMethod]
    public void TrackingObjectInit()
    {
        var tracking = ChangeTrackingProxyFactory.Create(new TrackingObject()
        {
            IntProperty = 1
        });

        Assert.AreEqual(1, tracking.IntProperty);
    }

    [TestMethod]
    public void NotTrackingObjectInit()
    {
        var value = new TrackingObject();
        value.NotTracking.Value = 1;
        var tracking = ChangeTrackingProxyFactory.Create(value);

        Assert.AreEqual(1, tracking.NotTracking.Value);
    }

    [TestMethod]
    public void NotTrackingPropertyObjectInit()
    {
        var value = new TrackingObject();
        value.NotTrackingProperty.Value = 1;
        var tracking = ChangeTrackingProxyFactory.Create(value);

        Assert.AreEqual(1, tracking.NotTrackingProperty.Value);
    }

    [TestMethod]
    public void ReadOnlyPropertyObjectInit()
    {
        var value = new TrackingObject();
        var tracking = ChangeTrackingProxyFactory.Create(value);

        Assert.AreEqual(1, tracking.ReadOnlyProperty);
    }

    [TestMethod]
    public void ReadOnlyVirtualPropertyObjectInit()
    {
        var value = new TrackingObject();
        var tracking = ChangeTrackingProxyFactory.Create(value);

        Assert.AreEqual(1, tracking.ReadOnlyVirtualProperty);
    }

    [TestMethod]
    public void RequirePropertyTrackingObject()
    {
        var value = new RequirePropertyTrackingObject
        {
            Array = [],
            Value = 1,
            Enumerable = []
        };
        var tracking = ChangeTrackingProxyFactory.Create(value);
        Assert.IsInstanceOfType<ChangeTrackingList<int>>(tracking.Enumerable);
    }
}

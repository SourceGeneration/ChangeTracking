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

}

[TestClass]
public class NotTrackingTest
{
    [TestMethod]
    public void NotTracking()
    {
        var tracking = ChangeTrackingProxyFactory.Create(new TrackingObject());
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsCascadingChanged);

        Assert.IsFalse(tracking.NotTracking is ICascadingChangeTracking);

        tracking.NotTracking.Value = 1;

        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsCascadingChanged);

        ((ICascadingChangeTracking)tracking).AcceptChanges();
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsCascadingChanged);
    }

    [TestMethod]
    public void CascadingTracking()
    {
        var tracking = ChangeTrackingProxyFactory.Create(new TrackingObject());
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsCascadingChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.CascadingTracking).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.CascadingTracking).IsCascadingChanged);

        tracking.CascadingTracking.Value = 1;

        Assert.IsTrue(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsTrue(((ICascadingChangeTracking)tracking).IsCascadingChanged);
        Assert.IsTrue(((ICascadingChangeTracking)tracking.CascadingTracking).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.CascadingTracking).IsCascadingChanged);

        ((ICascadingChangeTracking)tracking).AcceptChanges();
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsCascadingChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.CascadingTracking).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.CascadingTracking).IsCascadingChanged);
    }

    [TestMethod]
    public void NotTrackingProperty()
    {
        var tracking = ChangeTrackingProxyFactory.Create(new TrackingObject());
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsCascadingChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.NotTrackingProperty).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.NotTrackingProperty).IsCascadingChanged);

        tracking.NotTrackingProperty.Value = 1;

        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsCascadingChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.NotTrackingProperty).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.NotTrackingProperty).IsCascadingChanged);

        ((ICascadingChangeTracking)tracking).AcceptChanges();
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsCascadingChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.NotTrackingProperty).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.NotTrackingProperty).IsCascadingChanged);
    }
}

namespace SourceGeneration.States.Test.ChangeTracking;

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

namespace SourceGeneration.States.Test.ChangeTracking;

[TestClass]
public class NotTrackingTest
{
    [TestMethod]
    public void NotTracking()
    {
        var tracking = ChangeTrackingProxyFactory.Create(new NotTrackingWarpperObject());
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
        var tracking = ChangeTrackingProxyFactory.Create(new NotTrackingWarpperObject());
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
        var tracking = ChangeTrackingProxyFactory.Create(new NotTrackingWarpperObject());
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

[ChangeTracking]
public class NotTrackingWarpperObject
{
    public virtual NotTrackingObject NotTracking { get; set; } = new();
    public virtual NotTrackingPropertyObject NotTrackingProperty { get; set; } = new();
    public virtual CascadingTrackingObject CascadingTracking { get; set; } = new();
}

public class NotTrackingObject
{
    public virtual int Value { get; set; }
}

[ChangeTracking]
public class NotTrackingPropertyObject
{
    public int Value { get; set; }
}

[ChangeTracking]
public class CascadingTrackingObject
{
    public virtual int Value { get; set; }
}

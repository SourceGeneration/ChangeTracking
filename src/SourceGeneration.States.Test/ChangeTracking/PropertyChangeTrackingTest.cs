namespace SourceGeneration.States.Test.ChangeTracking;

[TestClass]
public class PropertyChangeTrackingTest
{
    [TestMethod]
    public void PropertyChanged()
    {
        var tracking = ChangeTrackingProxyFactory.Create(new TrackingObject());
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsCascadingChanged);

        tracking.IntProperty = 0;
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsCascadingChanged);

        tracking.IntProperty = 1;
        Assert.IsTrue(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsCascadingChanged);

        ((ICascadingChangeTracking)tracking).AcceptChanges();
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsCascadingChanged);
    }

    [TestMethod]
    public void CascadingObject_ValuePropertyChanged()
    {
        var tracking = ChangeTrackingProxyFactory.Create(new TrackingObject()
        {
            CascadingObject = new ()
        });
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsCascadingChanged);

        tracking.CascadingObject!.IntProperty = 0;
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsCascadingChanged);
        Assert.IsFalse(((ICascadingChangeTracking)(tracking.CascadingObject)).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)(tracking.CascadingObject)).IsCascadingChanged);

        tracking.CascadingObject!.IntProperty = 1;
        Assert.IsTrue(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsTrue(((ICascadingChangeTracking)tracking).IsCascadingChanged);
        Assert.IsTrue(((ICascadingChangeTracking)(tracking.CascadingObject)).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)(tracking.CascadingObject)).IsCascadingChanged);

        ((ICascadingChangeTracking)tracking).AcceptChanges();
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsCascadingChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.CascadingObject).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.CascadingObject).IsCascadingChanged);
    }

    [TestMethod]
    public void CascadingObject_ObjectPropertyChanged()
    {
        var tracking = ChangeTrackingProxyFactory.Create(new TrackingObject()
        {
            CascadingObject = new()
        });
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsCascadingChanged);

        tracking.CascadingObject = new TrackingObject();
        Assert.IsTrue(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsCascadingChanged);
        Assert.IsFalse(((ICascadingChangeTracking)(tracking.CascadingObject)).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)(tracking.CascadingObject)).IsCascadingChanged);


        ((ICascadingChangeTracking)tracking).AcceptChanges();
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsCascadingChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.CascadingObject).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.CascadingObject).IsCascadingChanged);

        tracking.CascadingObject.IntProperty = 1;
        Assert.IsTrue(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsTrue(((ICascadingChangeTracking)tracking).IsCascadingChanged);
        Assert.IsTrue(((ICascadingChangeTracking)tracking.CascadingObject).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.CascadingObject).IsCascadingChanged);

        ((ICascadingChangeTracking)tracking).AcceptChanges();
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsCascadingChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.CascadingObject).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.CascadingObject).IsCascadingChanged);
    }
}

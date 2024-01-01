namespace SourceGeneration.States.Test.ChangeTracking;


[TestClass]
public class InterfaceCollectionChangeTrackingTest
{
    [TestMethod]
    public void ValueList_Add()
    {
        var tracking = ChangeTrackingProxyFactory.Create(new TrackingObject());

        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsCascadingChanged);

        tracking.ICollectionOfValue.Add(1);
        Assert.IsTrue(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsTrue(((ICascadingChangeTracking)tracking).IsCascadingChanged);
        Assert.IsTrue(((ICascadingChangeTracking)tracking.ICollectionOfValue).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.ICollectionOfValue).IsCascadingChanged);

        ((ICascadingChangeTracking)tracking).AcceptChanges();
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsCascadingChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.ICollectionOfValue).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.ICollectionOfValue).IsCascadingChanged);
    }

    [TestMethod]
    public void ValueList_Remove()
    {
        var tracking = ChangeTrackingProxyFactory.Create(new TrackingObject()
        {
            ICollectionOfValue = [1]
        });

        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsCascadingChanged);

        tracking.ICollectionOfValue.Remove(1);
        Assert.IsTrue(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsTrue(((ICascadingChangeTracking)tracking).IsCascadingChanged);
        Assert.IsTrue(((ICascadingChangeTracking)tracking.ICollectionOfValue).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.ICollectionOfValue).IsCascadingChanged);

        ((ICascadingChangeTracking)tracking).AcceptChanges();
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsCascadingChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.ICollectionOfValue).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.ICollectionOfValue).IsCascadingChanged);
    }

    [TestMethod]
    public void ValueList_Clear()
    {
        var tracking = ChangeTrackingProxyFactory.Create(new TrackingObject()
        {
            ICollectionOfValue = [1]
        });

        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsCascadingChanged);

        tracking.ICollectionOfValue.Clear();
        Assert.IsTrue(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsTrue(((ICascadingChangeTracking)tracking).IsCascadingChanged);
        Assert.IsTrue(((ICascadingChangeTracking)tracking.ICollectionOfValue).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.ICollectionOfValue).IsCascadingChanged);

        ((ICascadingChangeTracking)tracking).AcceptChanges();
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsCascadingChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.ICollectionOfValue).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.ICollectionOfValue).IsCascadingChanged);
    }

    [TestMethod]
    public void ObjectList_ItemChanged()
    {
        var tracking = ChangeTrackingProxyFactory.Create(new TrackingObject()
        {
            ICollectionOfObject = [new TrackingObject(), new TrackingObject()]
        });

        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsCascadingChanged);

        tracking.ICollectionOfObject.First().IntProperty = 1;

        Assert.IsTrue(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsTrue(((ICascadingChangeTracking)tracking).IsCascadingChanged);

        Assert.IsTrue(((ICascadingChangeTracking)tracking.ICollectionOfObject).IsChanged);
        Assert.IsTrue(((ICascadingChangeTracking)tracking.ICollectionOfObject).IsCascadingChanged);

        Assert.IsTrue(((ICascadingChangeTracking)tracking.ICollectionOfObject.First()).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.ICollectionOfObject.First()).IsCascadingChanged);

        Assert.IsFalse(((ICascadingChangeTracking)tracking.ICollectionOfObject.Last()).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.ICollectionOfObject.Last()).IsCascadingChanged);

        ((ICascadingChangeTracking)tracking).AcceptChanges();

        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsCascadingChanged);

        Assert.IsFalse(((ICascadingChangeTracking)tracking.ICollectionOfObject).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.ICollectionOfObject).IsCascadingChanged);

        Assert.IsFalse(((ICascadingChangeTracking)tracking.ICollectionOfObject.First()).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.ICollectionOfObject.First()).IsCascadingChanged);

        Assert.IsFalse(((ICascadingChangeTracking)tracking.ICollectionOfObject.Last()).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.ICollectionOfObject.Last()).IsCascadingChanged);
    }
}

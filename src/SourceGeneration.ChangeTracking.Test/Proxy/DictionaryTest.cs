namespace SourceGeneration.ChangeTracking.Test;

[TestClass]
public class DictionaryTest
{
    [TestMethod]
    public void ValueDictionary_Add()
    {
        var tracking = ChangeTrackingProxyFactory.Create(new TrackingDictionaryObject());

        tracking.TrackingValueDictionary.Add(1, 1);
        Assert.IsTrue(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsTrue(((ICascadingChangeTracking)tracking).IsCascadingChanged);
        Assert.IsTrue(((ICascadingChangeTracking)tracking.TrackingValueDictionary).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.TrackingValueDictionary).IsCascadingChanged);

        ((ICascadingChangeTracking)tracking).AcceptChanges();
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsCascadingChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.TrackingValueDictionary).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.TrackingValueDictionary).IsCascadingChanged);
    }

    [TestMethod]
    public void ValueList_Remove()
    {
        var tracking = ChangeTrackingProxyFactory.Create(new TrackingDictionaryObject()
        {
            TrackingValueDictionary = new ChangeTrackingDictionary<int, int>
            {
                { 1, 1 },
            }
        });

        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsCascadingChanged);

        tracking.TrackingValueDictionary.Remove(1);
        Assert.IsTrue(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsTrue(((ICascadingChangeTracking)tracking).IsCascadingChanged);
        Assert.IsTrue(((ICascadingChangeTracking)tracking.TrackingValueDictionary).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.TrackingValueDictionary).IsCascadingChanged);

        ((ICascadingChangeTracking)tracking).AcceptChanges();
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsCascadingChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.TrackingValueDictionary).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.TrackingValueDictionary).IsCascadingChanged);
    }

    [TestMethod]
    public void ValueList_ItemChanged()
    {
        var tracking = ChangeTrackingProxyFactory.Create(new TrackingDictionaryObject()
        {
            TrackingValueDictionary = new ChangeTrackingDictionary<int, int>
            {
                { 1, 1 },
            }
        });

        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsCascadingChanged);

        tracking.TrackingValueDictionary[1] = 2;
        Assert.IsTrue(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsTrue(((ICascadingChangeTracking)tracking).IsCascadingChanged);
        Assert.IsTrue(((ICascadingChangeTracking)tracking.TrackingValueDictionary).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.TrackingValueDictionary).IsCascadingChanged);

        ((ICascadingChangeTracking)tracking).AcceptChanges();
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsCascadingChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.TrackingValueDictionary).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.TrackingValueDictionary).IsCascadingChanged);
    }

    [TestMethod]
    public void ValueList_Clear()
    {
        var tracking = ChangeTrackingProxyFactory.Create(new TrackingDictionaryObject()
        {
            TrackingValueDictionary = new ChangeTrackingDictionary<int, int>
            {
                { 1, 1 },
            }
        });

        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsCascadingChanged);

        tracking.TrackingValueDictionary.Clear();
        Assert.IsTrue(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsTrue(((ICascadingChangeTracking)tracking).IsCascadingChanged);
        Assert.IsTrue(((ICascadingChangeTracking)tracking.TrackingValueDictionary).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.TrackingValueDictionary).IsCascadingChanged);

        ((ICascadingChangeTracking)tracking).AcceptChanges();
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsCascadingChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.TrackingValueDictionary).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.TrackingValueDictionary).IsCascadingChanged);
    }

    [TestMethod]
    public void ObjectDictionary_ItemChanged()
    {
        var tracking = ChangeTrackingProxyFactory.Create(new TrackingDictionaryObject()
        {
            TrackingObjectDictionary = new ChangeTrackingDictionary<int, TrackingObject>
            {
                { 0, new TrackingObject() },
                { 1, new TrackingObject() }
            }
        });

        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsCascadingChanged);

        tracking.TrackingObjectDictionary[0].IntProperty = 1;

        Assert.IsTrue(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsTrue(((ICascadingChangeTracking)tracking).IsCascadingChanged);

        Assert.IsTrue(((ICascadingChangeTracking)tracking.TrackingObjectDictionary).IsChanged);
        Assert.IsTrue(((ICascadingChangeTracking)tracking.TrackingObjectDictionary).IsCascadingChanged);

        Assert.IsTrue(((ICascadingChangeTracking)tracking.TrackingObjectDictionary[0]).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.TrackingObjectDictionary[0]).IsCascadingChanged);

        Assert.IsFalse(((ICascadingChangeTracking)tracking.TrackingObjectDictionary[1]).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.TrackingObjectDictionary[1]).IsCascadingChanged);

        ((ICascadingChangeTracking)tracking).AcceptChanges();

        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsCascadingChanged);

        Assert.IsFalse(((ICascadingChangeTracking)tracking.TrackingObjectDictionary).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.TrackingObjectDictionary).IsCascadingChanged);

        Assert.IsFalse(((ICascadingChangeTracking)tracking.TrackingObjectDictionary[0]).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.TrackingObjectDictionary[0]).IsCascadingChanged);

        Assert.IsFalse(((ICascadingChangeTracking)tracking.TrackingObjectDictionary[1]).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.TrackingObjectDictionary[1]).IsCascadingChanged);
    }

}

[ChangeTracking]
public class TrackingDictionaryObject
{
    public virtual ChangeTrackingDictionary<int, int> TrackingValueDictionary { get; set; } = [];
    public virtual ChangeTrackingDictionary<int, TrackingObject> TrackingObjectDictionary { get; set; } = [];
}

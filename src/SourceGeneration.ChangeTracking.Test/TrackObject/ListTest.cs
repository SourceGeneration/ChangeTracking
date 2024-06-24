using SourceGeneration.ChangeTracking.Test;

namespace SourceGeneration.ChangeTracking.TrackObjects;

[TestClass]
public class ListTest
{
    [TestMethod]
    public void ValueList_Add()
    {
        var tracking = ChangeTrackingProxyFactory.Create(new ListTestObject());

        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsCascadingChanged);

        tracking.TrackingValueList.Add(1);
        Assert.IsTrue(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsTrue(((ICascadingChangeTracking)tracking).IsCascadingChanged);
        Assert.IsTrue(((ICascadingChangeTracking)tracking.TrackingValueList).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.TrackingValueList).IsCascadingChanged);

        ((ICascadingChangeTracking)tracking).AcceptChanges();
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsCascadingChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.TrackingValueList).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.TrackingValueList).IsCascadingChanged);
    }

    [TestMethod]
    public void ValueList_Remove()
    {
        var tracking = ChangeTrackingProxyFactory.Create(new ListTestObject()
        {
            TrackingValueList = [1]
        });

        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsCascadingChanged);

        tracking.TrackingValueList.RemoveAt(0);
        Assert.IsTrue(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsTrue(((ICascadingChangeTracking)tracking).IsCascadingChanged);
        Assert.IsTrue(((ICascadingChangeTracking)tracking.TrackingValueList).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.TrackingValueList).IsCascadingChanged);

        ((ICascadingChangeTracking)tracking).AcceptChanges();
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsCascadingChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.TrackingValueList).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.TrackingValueList).IsCascadingChanged);
    }

    [TestMethod]
    public void ValueList_Clear()
    {
        var tracking = ChangeTrackingProxyFactory.Create(new ListTestObject()
        {
            TrackingValueList = [1]
        });

        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsCascadingChanged);

        tracking.TrackingValueList.Clear();
        Assert.IsTrue(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsTrue(((ICascadingChangeTracking)tracking).IsCascadingChanged);
        Assert.IsTrue(((ICascadingChangeTracking)tracking.TrackingValueList).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.TrackingValueList).IsCascadingChanged);

        ((ICascadingChangeTracking)tracking).AcceptChanges();
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsCascadingChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.TrackingValueList).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.TrackingValueList).IsCascadingChanged);
    }

    [TestMethod]
    public void ValueList_ItemChanged()
    {
        var tracking = ChangeTrackingProxyFactory.Create(new ListTestObject()
        {
            TrackingValueList = [1]
        });

        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsCascadingChanged);

        tracking.TrackingValueList[0] = 2;
        Assert.IsTrue(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsTrue(((ICascadingChangeTracking)tracking).IsCascadingChanged);
        Assert.IsTrue(((ICascadingChangeTracking)tracking.TrackingValueList).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.TrackingValueList).IsCascadingChanged);

        ((ICascadingChangeTracking)tracking).AcceptChanges();
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsCascadingChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.TrackingValueList).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.TrackingValueList).IsCascadingChanged);
    }

    [TestMethod]
    public void ObjectList_ItemChanged()
    {
        var tracking = ChangeTrackingProxyFactory.Create(new ListTestObject()
        {
            TrackingObjectList = [new TrackingObject(), new TrackingObject()]
        });

        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsCascadingChanged);

        Assert.IsFalse(((ICascadingChangeTracking)tracking.TrackingObjectList).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.TrackingObjectList).IsCascadingChanged);

        Assert.IsFalse(((ICascadingChangeTracking)tracking.TrackingObjectList[0]).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.TrackingObjectList[0]).IsCascadingChanged);

        Assert.IsFalse(((ICascadingChangeTracking)tracking.TrackingObjectList[1]).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.TrackingObjectList[1]).IsCascadingChanged);

        tracking.TrackingObjectList[0].IntProperty++;


        Assert.IsTrue(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsTrue(((ICascadingChangeTracking)tracking).IsCascadingChanged);

        Assert.IsTrue(((ICascadingChangeTracking)tracking.TrackingObjectList).IsChanged);
        Assert.IsTrue(((ICascadingChangeTracking)tracking.TrackingObjectList).IsCascadingChanged);

        Assert.IsTrue(((ICascadingChangeTracking)tracking.TrackingObjectList[0]).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.TrackingObjectList[0]).IsCascadingChanged);

        Assert.IsFalse(((ICascadingChangeTracking)tracking.TrackingObjectList[1]).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.TrackingObjectList[1]).IsCascadingChanged);

        ((ICascadingChangeTracking)tracking).AcceptChanges();

        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsCascadingChanged);

        Assert.IsFalse(((ICascadingChangeTracking)tracking.TrackingObjectList).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.TrackingObjectList).IsCascadingChanged);

        Assert.IsFalse(((ICascadingChangeTracking)tracking.TrackingObjectList[0]).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.TrackingObjectList[0]).IsCascadingChanged);

        Assert.IsFalse(((ICascadingChangeTracking)tracking.TrackingObjectList[1]).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.TrackingObjectList[1]).IsCascadingChanged);
    }
}

[ChangeTracking]
public class ListTestObject
{
    public virtual ChangeTrackingList<int> TrackingValueList { get; set; } = [];
    public virtual ChangeTrackingList<TrackingObject> TrackingObjectList { get; set; } = [];
}

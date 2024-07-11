using SourceGeneration.ChangeTracking.Test;

namespace SourceGeneration.ChangeTracking.TrackObjects;

[TestClass]
public class ListInterfaceTest
{
    [TestMethod]
    public void ValueList_Add()
    {
        var tracking = new TrackingListInterfaceObject();
        ((ICascadingChangeTracking)tracking).AcceptChanges();

        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsCascadingChanged);

        tracking.IListOfValue.Add(1);
        Assert.IsTrue(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsTrue(((ICascadingChangeTracking)tracking).IsCascadingChanged);
        Assert.IsTrue(((ICascadingChangeTracking)tracking.IListOfValue).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.IListOfValue).IsCascadingChanged);

        ((ICascadingChangeTracking)tracking).AcceptChanges();
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsCascadingChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.IListOfValue).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.IListOfValue).IsCascadingChanged);
    }

    [TestMethod]
    public void ValueList_Remove()
    {
        var tracking = new TrackingListInterfaceObject
        {
            IListOfValue = [1]
        };
        ((ICascadingChangeTracking)tracking).AcceptChanges();

        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsCascadingChanged);

        tracking.IListOfValue.RemoveAt(0);
        Assert.IsTrue(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsTrue(((ICascadingChangeTracking)tracking).IsCascadingChanged);
        Assert.IsTrue(((ICascadingChangeTracking)tracking.IListOfValue).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.IListOfValue).IsCascadingChanged);

        ((ICascadingChangeTracking)tracking).AcceptChanges();
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsCascadingChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.IListOfValue).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.IListOfValue).IsCascadingChanged);
    }

    [TestMethod]
    public void ValueList_Clear()
    {
        var tracking = new TrackingListInterfaceObject
        {
            IListOfValue = [1]
        };
        ((ICascadingChangeTracking)tracking).AcceptChanges();

        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsCascadingChanged);

        tracking.IListOfValue.Clear();
        Assert.IsTrue(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsTrue(((ICascadingChangeTracking)tracking).IsCascadingChanged);
        Assert.IsTrue(((ICascadingChangeTracking)tracking.IListOfValue).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.IListOfValue).IsCascadingChanged);

        ((ICascadingChangeTracking)tracking).AcceptChanges();
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsCascadingChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.IListOfValue).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.IListOfValue).IsCascadingChanged);
    }

    [TestMethod]
    public void ValueList_ItemChanged()
    {
        var tracking = new TrackingListInterfaceObject
        {
            IListOfValue = [1]
        };
        ((ICascadingChangeTracking)tracking).AcceptChanges();

        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsCascadingChanged);

        tracking.IListOfValue[0] = 2;
        Assert.IsTrue(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsTrue(((ICascadingChangeTracking)tracking).IsCascadingChanged);
        Assert.IsTrue(((ICascadingChangeTracking)tracking.IListOfValue).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.IListOfValue).IsCascadingChanged);

        ((ICascadingChangeTracking)tracking).AcceptChanges();
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsCascadingChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.IListOfValue).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.IListOfValue).IsCascadingChanged);
    }

    [TestMethod]
    public void ObjectList_ItemChanged()
    {
        var tracking = new TrackingListInterfaceObject()
        {
            IListOfObject = [new TrackingObject(), new TrackingObject()]
        };
        ((ICascadingChangeTracking)tracking).AcceptChanges();

        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsCascadingChanged);

        tracking.IListOfObject[0].IntProperty = 1;

        Assert.IsTrue(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsTrue(((ICascadingChangeTracking)tracking).IsCascadingChanged);

        Assert.IsTrue(((ICascadingChangeTracking)tracking.IListOfObject).IsChanged);
        Assert.IsTrue(((ICascadingChangeTracking)tracking.IListOfObject).IsCascadingChanged);

        Assert.IsTrue(((ICascadingChangeTracking)tracking.IListOfObject[0]).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.IListOfObject[0]).IsCascadingChanged);

        Assert.IsFalse(((ICascadingChangeTracking)tracking.IListOfObject[1]).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.IListOfObject[1]).IsCascadingChanged);

        ((ICascadingChangeTracking)tracking).AcceptChanges();

        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsCascadingChanged);

        Assert.IsFalse(((ICascadingChangeTracking)tracking.IListOfObject).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.IListOfObject).IsCascadingChanged);

        Assert.IsFalse(((ICascadingChangeTracking)tracking.IListOfObject[0]).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.IListOfObject[0]).IsCascadingChanged);

        Assert.IsFalse(((ICascadingChangeTracking)tracking.IListOfObject[1]).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.IListOfObject[1]).IsCascadingChanged);
    }
}

[ChangeTracking]
public partial class TrackingListInterfaceObject
{
    public TrackingListInterfaceObject()
    {
        IListOfValue = [];
        IListOfObject = [];
    }

    public partial IList<int> IListOfValue { get; set; } 
    public partial IList<TrackingObject> IListOfObject { get; set; } 
}

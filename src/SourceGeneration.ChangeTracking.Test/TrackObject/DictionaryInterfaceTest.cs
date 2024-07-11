using SourceGeneration.ChangeTracking.Test;

namespace SourceGeneration.ChangeTracking.TrackObjects;

[TestClass]
public class DictionaryInterfaceTest
{
    [TestMethod]
    public void ValueDictionary_Add()
    {
        var tracking = new TrackingInterfaceDictionaryObject();

        tracking.IDictionaryOfValue.Add(1, 1);
        Assert.IsTrue(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsTrue(((ICascadingChangeTracking)tracking).IsCascadingChanged);
        Assert.IsTrue(((ICascadingChangeTracking)tracking.IDictionaryOfValue).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.IDictionaryOfValue).IsCascadingChanged);

        ((ICascadingChangeTracking)tracking).AcceptChanges();
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsCascadingChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.IDictionaryOfValue).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.IDictionaryOfValue).IsCascadingChanged);
    }

    [TestMethod]
    public void ValueList_Remove()
    {
        var tracking = new TrackingInterfaceDictionaryObject
        {
            IDictionaryOfValue = new Dictionary<int, int>
            {
                { 1, 1 },
            }
        };
        ((ICascadingChangeTracking)tracking).AcceptChanges();

        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsCascadingChanged);

        tracking.IDictionaryOfValue.Remove(1);
        Assert.IsTrue(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsTrue(((ICascadingChangeTracking)tracking).IsCascadingChanged);
        Assert.IsTrue(((ICascadingChangeTracking)tracking.IDictionaryOfValue).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.IDictionaryOfValue).IsCascadingChanged);

        ((ICascadingChangeTracking)tracking).AcceptChanges();
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsCascadingChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.IDictionaryOfValue).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.IDictionaryOfValue).IsCascadingChanged);
    }

    [TestMethod]
    public void ValueList_ItemChanged()
    {
        var tracking = new TrackingInterfaceDictionaryObject()
        {
            IDictionaryOfValue = new Dictionary<int, int>
            {
                { 1, 1 },
            }
        };
        ((ICascadingChangeTracking)tracking).AcceptChanges();

        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsCascadingChanged);

        tracking.IDictionaryOfValue[1] = 2;
        Assert.IsTrue(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsTrue(((ICascadingChangeTracking)tracking).IsCascadingChanged);
        Assert.IsTrue(((ICascadingChangeTracking)tracking.IDictionaryOfValue).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.IDictionaryOfValue).IsCascadingChanged);

        ((ICascadingChangeTracking)tracking).AcceptChanges();
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsCascadingChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.IDictionaryOfValue).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.IDictionaryOfValue).IsCascadingChanged);
    }

    [TestMethod]
    public void ObjectDictionary_ItemChanged()
    {
        var tracking = new TrackingInterfaceDictionaryObject()
        {
            IDictionaryOfObject = new Dictionary<int, TrackingObject>
            {
                { 0, new TrackingObject() },
                { 1, new TrackingObject() }
            }
        };
        ((ICascadingChangeTracking)tracking).AcceptChanges();

        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsCascadingChanged);

        tracking.IDictionaryOfObject[0].IntProperty = 1;

        Assert.IsTrue(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsTrue(((ICascadingChangeTracking)tracking).IsCascadingChanged);

        Assert.IsTrue(((ICascadingChangeTracking)tracking.IDictionaryOfObject).IsChanged);
        Assert.IsTrue(((ICascadingChangeTracking)tracking.IDictionaryOfObject).IsCascadingChanged);

        Assert.IsTrue(((ICascadingChangeTracking)tracking.IDictionaryOfObject[0]).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.IDictionaryOfObject[0]).IsCascadingChanged);

        Assert.IsFalse(((ICascadingChangeTracking)tracking.IDictionaryOfObject[1]).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.IDictionaryOfObject[1]).IsCascadingChanged);

        ((ICascadingChangeTracking)tracking).AcceptChanges();

        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsCascadingChanged);

        Assert.IsFalse(((ICascadingChangeTracking)tracking.IDictionaryOfObject).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.IDictionaryOfObject).IsCascadingChanged);

        Assert.IsFalse(((ICascadingChangeTracking)tracking.IDictionaryOfObject[0]).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.IDictionaryOfObject[0]).IsCascadingChanged);

        Assert.IsFalse(((ICascadingChangeTracking)tracking.IDictionaryOfObject[1]).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.IDictionaryOfObject[1]).IsCascadingChanged);
    }

}

[ChangeTracking]
public partial class TrackingInterfaceDictionaryObject
{

    public TrackingInterfaceDictionaryObject()
    {
        IDictionaryOfValue = new Dictionary<int, int>();
        IDictionaryOfObject  = new Dictionary<int, TrackingObject>();
    }

    public partial IDictionary<int, int> IDictionaryOfValue { get; set; }
    public partial IDictionary<int, TrackingObject> IDictionaryOfObject { get; set; }
}

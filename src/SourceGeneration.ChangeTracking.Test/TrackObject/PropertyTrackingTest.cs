using SourceGeneration.ChangeTracking.Test;

namespace SourceGeneration.ChangeTracking.TrackObjects;

[TestClass]
public class PropertyTrackingTest
{
    [TestMethod]
    public void PropertyChanged()
    {
        var tracking = new TrackingPropertyObject();
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
        var tracking = new TrackingPropertyObject()
        {
            CascadingObject = new()
        };
        ((ICascadingChangeTracking)tracking).AcceptChanges();

        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsCascadingChanged);

        tracking.CascadingObject!.IntProperty = 0;
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsCascadingChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.CascadingObject).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.CascadingObject).IsCascadingChanged);

        tracking.CascadingObject!.IntProperty = 1;
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

    [TestMethod]
    public void CascadingObject_ObjectPropertyChanged()
    {
        var tracking = new TrackingPropertyObject()
        {
            CascadingObject = new()
        }; 
        ((ICascadingChangeTracking)tracking).AcceptChanges();

        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsCascadingChanged);

        tracking.CascadingObject = new TrackingObject();
        Assert.IsTrue(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsCascadingChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.CascadingObject).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.CascadingObject).IsCascadingChanged);


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

[ChangeTracking]
public partial class TrackingPropertyObject
{
    public partial string? StringProperty { get; set; }
    public partial int IntProperty { get; set; }
    public partial TrackingObject? CascadingObject { get; set; }

}
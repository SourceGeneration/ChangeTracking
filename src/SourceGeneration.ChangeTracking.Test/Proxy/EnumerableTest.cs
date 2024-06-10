namespace SourceGeneration.ChangeTracking.Test;

[TestClass]
public class EnumerableTest
{
    [TestMethod]
    public void ObjectList_ItemChanged()
    {
        var tracking = ChangeTrackingProxyFactory.Create(new TrackingEnumerableObject()
        {
            IEnumerableOfObject = [new TrackingObject(), new TrackingObject()]
        });

        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsCascadingChanged);

        tracking.IEnumerableOfObject.First().IntProperty = 1;

        Assert.IsTrue(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsTrue(((ICascadingChangeTracking)tracking).IsCascadingChanged);

        Assert.IsTrue(((ICascadingChangeTracking)tracking.IEnumerableOfObject).IsChanged);
        Assert.IsTrue(((ICascadingChangeTracking)tracking.IEnumerableOfObject).IsCascadingChanged);

        Assert.IsTrue(((ICascadingChangeTracking)tracking.IEnumerableOfObject.First()).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.IEnumerableOfObject.First()).IsCascadingChanged);

        Assert.IsFalse(((ICascadingChangeTracking)tracking.IEnumerableOfObject.Last()).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.IEnumerableOfObject.Last()).IsCascadingChanged);

        ((ICascadingChangeTracking)tracking).AcceptChanges();

        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsCascadingChanged);

        Assert.IsFalse(((ICascadingChangeTracking)tracking.IEnumerableOfObject).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.IEnumerableOfObject).IsCascadingChanged);

        Assert.IsFalse(((ICascadingChangeTracking)tracking.IEnumerableOfObject.First()).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.IEnumerableOfObject.First()).IsCascadingChanged);

        Assert.IsFalse(((ICascadingChangeTracking)tracking.IEnumerableOfObject.Last()).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking.IEnumerableOfObject.Last()).IsCascadingChanged);
    }
}

[ChangeTracking]
public class TrackingEnumerableObject
{
    public virtual IEnumerable<int> IEnumerableOfValue { get; set; } = [];
    public virtual IEnumerable<TrackingObject> IEnumerableOfObject { get; set; } = [];

}

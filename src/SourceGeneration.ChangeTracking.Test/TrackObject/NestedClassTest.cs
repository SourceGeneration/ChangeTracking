using System.ComponentModel;

namespace SourceGeneration.ChangeTracking.TrackObjects;

[TestClass]
public class NestedClassTest
{
    [TestMethod]
    public void NestedClass()
    {
        var model = ChangeTrackingProxyFactory.Create(new NestedClassTestObject());
        model.Value = 1;
        Assert.IsInstanceOfType<IChangeTracking>(model);
        Assert.IsTrue(((IChangeTracking)model).IsChanged);
    }


    [ChangeTracking]
    public class NestedClassTestObject
    {
        public virtual int Value { get; set; }
    }
}

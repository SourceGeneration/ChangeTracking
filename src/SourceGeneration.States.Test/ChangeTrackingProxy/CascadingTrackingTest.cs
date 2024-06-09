using System.ComponentModel;

namespace SourceGeneration.States.Test.ChangeTrackingProxy;

[TestClass]
public class CascadingTrackingTest
{
    [TestMethod]
    public void Cascading()
    {
        var model = ChangeTrackingProxyFactory.Create(new CascadingTestObject());
        model.Object.List.Add(1);
        Assert.IsTrue(((IChangeTracking)model.Object).IsChanged);
        Assert.IsTrue(((IChangeTracking)model).IsChanged);
    }
}

[ChangeTracking]
public class CascadingTestObject
{
    public virtual CascadingCollectionTestObject Object { get; set; } = new();
}

[ChangeTracking]
public class CascadingCollectionTestObject
{
    public virtual ChangeTrackingList<int> List { get; set; } = [];
}


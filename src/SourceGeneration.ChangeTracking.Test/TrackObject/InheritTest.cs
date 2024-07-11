using SourceGeneration.ChangeTracking.Test;

namespace SourceGeneration.ChangeTracking.TrackObjects;

[TestClass]
public class InheritTest
{
    [TestMethod]
    public void BaseValueChange()
    {
        ChildObject child = new()
        {
            BaseProperty = 1
        };
        Assert.IsTrue(((ICascadingChangeTracking)child).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)child).IsCascadingChanged);

        ((ICascadingChangeTracking)child).AcceptChanges();
        Assert.IsFalse(((ICascadingChangeTracking)child).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)child).IsCascadingChanged);
    }

    [TestMethod]
    public void InheritValueChange()
    {
        ChildObject child = new()
        {
            ChildProperty = 1
        };
        Assert.IsTrue(((ICascadingChangeTracking)child).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)child).IsCascadingChanged);

        ((ICascadingChangeTracking)child).AcceptChanges();
        Assert.IsFalse(((ICascadingChangeTracking)child).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)child).IsCascadingChanged);
    }

    [TestMethod]
    public void BaseCascadingChange()
    {
        ChildObject child = new();

        child.BaseObjectProperty.IntProperty = 1;
        Assert.IsTrue(((ICascadingChangeTracking)child).IsChanged);
        Assert.IsTrue(((ICascadingChangeTracking)child).IsCascadingChanged);

        ((ICascadingChangeTracking)child).AcceptChanges();
        Assert.IsFalse(((ICascadingChangeTracking)child).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)child).IsCascadingChanged);
    }

    [TestMethod]
    public void InheritCascadingChange()
    {
        ChildObject child = new();
        child.ChildObjectProperty.IntProperty = 1;
        Assert.IsTrue(((ICascadingChangeTracking)child).IsChanged);
        Assert.IsTrue(((ICascadingChangeTracking)child).IsCascadingChanged);

        ((ICascadingChangeTracking)child).AcceptChanges();
        Assert.IsFalse(((ICascadingChangeTracking)child).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)child).IsCascadingChanged);
    }

    [TestMethod]
    public void Inherit2CascadingChange()
    {
        Child2Object child = new();
        child.Child2ObjectProperty.IntProperty = 1;
        Assert.IsTrue(((ICascadingChangeTracking)child).IsChanged);
        Assert.IsTrue(((ICascadingChangeTracking)child).IsCascadingChanged);

        ((ICascadingChangeTracking)child).AcceptChanges();
        Assert.IsFalse(((ICascadingChangeTracking)child).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)child).IsCascadingChanged);
    }
}

[ChangeTracking]
public partial class BaseObject
{
    public BaseObject()
    {
        BaseObjectProperty = new();
    }
    public partial int BaseProperty { get; set; }
    public partial TrackingObject BaseObjectProperty { get; set; }
}

[ChangeTracking]
public partial class ChildObject : BaseObject
{
    public ChildObject()
    {
        ChildObjectProperty = new();
    }
    public partial int ChildProperty { get; set; }

    public partial TrackingObject ChildObjectProperty { get; set; }
}

[ChangeTracking]
public partial class Child2Object : ChildObject
{
    public Child2Object()
    {
        Child2ObjectProperty = new();
    }
    public partial int Child2Property { get; set; }

    public partial TrackingObject Child2ObjectProperty { get; set; }
}
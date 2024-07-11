﻿using SourceGeneration.ChangeTracking.Test;
using System.ComponentModel;

namespace SourceGeneration.ChangeTracking.TrackObjects;

[TestClass]
public class CascadingTrackingTest
{
    [TestMethod]
    public void Cascading()
    {
        var model = new CascadingTestObject();
        ((IChangeTracking)model).AcceptChanges();
        model.Object.Values.Add(1);
        Assert.IsTrue(((IChangeTracking)model.Object).IsChanged);
        Assert.IsTrue(((IChangeTracking)model).IsChanged);
    }

    [TestMethod]
    public void Cascading_2()
    {
        var model = new CascadingTestObject();
        ((IChangeTracking)model).AcceptChanges();

        Assert.IsFalse(((IChangeTracking)model.Object.Objects).IsChanged);
        Assert.IsFalse(((IChangeTracking)model.Object).IsChanged);
        Assert.IsFalse(((IChangeTracking)model).IsChanged);

        model.Object.Objects[0].IntProperty = 1;

        Assert.IsTrue(((IChangeTracking)model.Object.Objects).IsChanged);
        Assert.IsTrue(((IChangeTracking)model.Object).IsChanged);
        Assert.IsTrue(((IChangeTracking)model).IsChanged);

        Assert.IsFalse(((ICascadingChangeTracking)model.Object.Objects[0]).IsCascadingChanged);
        Assert.IsTrue(((ICascadingChangeTracking)model.Object.Objects).IsCascadingChanged);
        Assert.IsTrue(((ICascadingChangeTracking)model.Object).IsCascadingChanged);
        Assert.IsTrue(((ICascadingChangeTracking)model).IsCascadingChanged);
    }

}

[ChangeTracking]
public partial class CascadingTestObject
{
    public CascadingTestObject()
    {
        Object = new();
    }

    public partial CascadingCollectionTestObject Object { get; set; }
}

[ChangeTracking]
public partial class CascadingCollectionTestObject
{
    public CascadingCollectionTestObject()
    {
        Values = [];
        Objects = [new TrackingObject()];
    }

    public partial ChangeTrackingList<int> Values { get; set; }
    public partial ChangeTrackingList<TrackingObject> Objects { get; set; }
}

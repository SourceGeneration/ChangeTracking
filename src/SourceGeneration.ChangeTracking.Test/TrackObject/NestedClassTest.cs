using System.ComponentModel;
using static SourceGeneration.ChangeTracking.TrackObjects.NestedClassTest.NestedClassTestObject;

namespace SourceGeneration.ChangeTracking.TrackObjects;

[TestClass]
public partial class NestedClassTest
{
    [TestMethod]
    public void NestedClass()
    {
        var model = new NestedClassTestObject();
        ((IChangeTracking)model).AcceptChanges();
        model.Value = 1;
        Assert.IsTrue(((IChangeTracking)model).IsChanged);
    }

    [TestMethod]
    public void Nested2Class()
    {
        var model = new Nested2ClassTestObject();
        ((IChangeTracking)model).AcceptChanges();
        model.Value2 = 1;
        Assert.IsTrue(((IChangeTracking)model).IsChanged);
    }

    [ChangeTracking]
    public partial class NestedClassTestObject
    {
        public partial int Value { get; set; }

        [ChangeTracking]
        public partial class Nested2ClassTestObject
        {
            public partial int Value2 { get; set; }
        }

    }
}

namespace SourceGeneration.ChangeTracking.TrackObjects;

[TestClass]
public partial class PropertyInitializerTest
{
    [TestMethod]
    public void InitializerTest()
    {
        DefaultValueObject o = new();

        Assert.IsFalse(((ICascadingChangeTracking)o).IsBaseChanged);
        Assert.IsFalse(((ICascadingChangeTracking)o).IsCascadingChanged);

        o.Value1.Value = 1;

        Assert.IsFalse(((ICascadingChangeTracking)o).IsBaseChanged);
        Assert.IsTrue(((ICascadingChangeTracking)o).IsCascadingChanged);

        o.AcceptChanges();

        Assert.IsFalse(((ICascadingChangeTracking)o).IsBaseChanged);
        Assert.IsFalse(((ICascadingChangeTracking)o).IsCascadingChanged);

    }


    [ChangeTracking]
    public partial class DefaultValueObject : State
    {
        public partial InnerObject Value1 { get; set; } = new();
        public partial InnerObject Value2 { get; set; }
    }

    [ChangeTracking]
    public partial class InnerObject
    {
        public partial int Value { get; set; }
    }


}


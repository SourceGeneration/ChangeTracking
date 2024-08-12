namespace SourceGeneration.ChangeTracking;

[TestClass]
public class GenericTypeTest
{
    [TestMethod]
    public void Test()
    {
        GenericType<int> tracking = new();

        ((ICascadingChangeTracking)tracking).AcceptChanges();

        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsCascadingChanged);

        tracking.A = 1;
        Assert.IsTrue(((ICascadingChangeTracking)tracking).IsChanged);
        Assert.IsFalse(((ICascadingChangeTracking)tracking).IsCascadingChanged);

    }
}

[ChangeTracking]
public partial class GenericType<T>
{
    public partial int A { get; set; }
}

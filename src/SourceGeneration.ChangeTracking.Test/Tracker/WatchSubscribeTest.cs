namespace SourceGeneration.ChangeTracking.Tracker;

[TestClass]
public class WatchSubscribeTest
{
    [TestMethod]
    public void Watch()
    {
        var state = new TrackingTarget
        {
            Value1 = 1,
            Value2 = "a",
        };
        ChangeTracker<TrackingTarget> tracker = new(state);

        int? value1 = null;
        string? value2 = null;
        tracker.Watch(x => x.Value1, x => value1 = x);
        tracker.Watch(x => x.Value2, x => value2 = x);

        Assert.AreEqual(1, value1);
        Assert.AreEqual("a", value2);
    }

    public class TrackingTarget
    {
        public int? Value1 { get; set; }
        public string? Value2 { get; set; }
    }

}

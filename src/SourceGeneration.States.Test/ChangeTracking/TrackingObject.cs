namespace SourceGeneration.States.Test.ChangeTracking;

[ChangeTracking]
public class TrackingObject
{
    public virtual string? StringProperty { get; set; }
    public virtual int IntProperty { get; set; }

    public virtual TrackingObject? CascadingObject { get; set; }

    public virtual ChangeTrackingList<int> TrackingValueList { get; set; } = [];
    public virtual ChangeTrackingList<TrackingObject> TrackingObjectList { get; set; } = [];

    public virtual ChangeTrackingDictionary<int, int> TrackingValueDictionary { get; set; } = [];
    public virtual ChangeTrackingDictionary<int, TrackingObject> TrackingObjectDictionary { get; set; } = [];

    public virtual NotTrackingObject NotTracking { get; set; } = new();
    public virtual CascadingTrackingObject CascadingTracking { get; set; } = new();
    public virtual NotTrackingPropertyObject NotTrackingProperty { get; set; } = new();

    public int ReadOnlyProperty { get; } = 1;
    public virtual int ReadOnlyVirtualProperty { get; } = 1;
}

public class NotTrackingObject
{
    public virtual int Value { get; set; }
}

[ChangeTracking]
public class NotTrackingPropertyObject
{
    public int Value { get; set; }
}

[ChangeTracking]
public class CascadingTrackingObject
{
    public virtual int Value { get; set; }
}
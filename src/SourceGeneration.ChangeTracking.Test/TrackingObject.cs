namespace SourceGeneration.ChangeTracking.Test;

[ChangeTracking]
public partial class TrackingObject
{
    public partial string? StringProperty { get; set; }
    public partial int IntProperty { get; set; }

    public partial TrackingObject? CascadingObject { get; set; }
}

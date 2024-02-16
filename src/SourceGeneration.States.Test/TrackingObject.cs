namespace SourceGeneration.States.Test;

[ChangeTracking]
public class TrackingObject
{
    public virtual string? StringProperty { get; set; }
    public virtual int IntProperty { get; set; }

    public virtual TrackingObject? CascadingObject { get; set; }
}

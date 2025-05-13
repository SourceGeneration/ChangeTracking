namespace SourceGeneration.ChangeTracking;

public enum ChangeTrackingScope
{
    Instance = 0,
    InstanceProperty = 1,
    Cascading = 2,
    Always = 3,
}
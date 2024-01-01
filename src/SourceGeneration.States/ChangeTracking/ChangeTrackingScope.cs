namespace SourceGeneration.States;

public enum ChangeTrackingScope
{
    RootChanged = 0,
    RootOrCascadingChanged = 1,
    Always = 2,
}
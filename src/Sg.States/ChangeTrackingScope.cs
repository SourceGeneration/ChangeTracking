namespace Sg.States;

public enum ChangeTrackingScope
{
    RootChanged = 0,
    RootOrSubsetChanged = 1,
    Always = 2,
}
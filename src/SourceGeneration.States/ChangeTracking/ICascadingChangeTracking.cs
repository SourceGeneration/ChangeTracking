using System.ComponentModel;

namespace SourceGeneration.States;

public interface ICascadingChangeTracking : IChangeTracking
{
    bool IsCascadingChanged { get; }
}

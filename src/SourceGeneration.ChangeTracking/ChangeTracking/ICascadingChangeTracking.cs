using System.ComponentModel;

namespace SourceGeneration.ChangeTracking;

public interface ICascadingChangeTracking : IChangeTracking
{
    bool IsCascadingChanged { get; }
    bool IsBaseChanged { get; }
}

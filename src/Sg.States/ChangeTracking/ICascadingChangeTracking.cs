using System.ComponentModel;

namespace SourceGeneration.States;

public interface ICascadingChangeTracking : IChangeTracking
{
    bool IsBaseChanged { get; }
    bool IsItemChanged { get; }
}

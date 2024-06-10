using System.Collections.Generic;
using System.ComponentModel;

namespace SourceGeneration.ChangeTracking;

public sealed class ChangeTrackingScopeEqualityComparer<TValue>(ChangeTrackingScope changeTrackingScope) : IEqualityComparer<TValue>
{
    public bool Equals(TValue? x, TValue? y)
    {
        if (changeTrackingScope == ChangeTrackingScope.Always)
            return false;

        if (x == null && y == null) return true;
        if (x == null || y == null) return false;

        if (!EqualityComparer<TValue>.Default.Equals(x, y))
            return false;

        if (y is IChangeTracking tracking && tracking.IsChanged)
        {
            if (changeTrackingScope == ChangeTrackingScope.Root)
            {
                if (y is ICascadingChangeTracking cascading)
                    return cascading.IsCascadingChanged;
            }
            else
            {
                return false;
            }
        }

        return true;
    }

    public int GetHashCode(TValue obj) => obj?.GetHashCode() ?? 0;
}


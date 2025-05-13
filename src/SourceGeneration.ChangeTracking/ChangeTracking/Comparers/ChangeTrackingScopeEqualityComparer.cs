using System.Collections.Generic;
using System.ComponentModel;

namespace SourceGeneration.ChangeTracking;

public sealed class ChangeTrackingScopeEqualityComparer<TValue>(ChangeTrackingScope changeTrackingScope) : IEqualityComparer<TValue>
{
    public bool Equals(TValue? oldValue, TValue? newValue)
    {
        if (changeTrackingScope == ChangeTrackingScope.Always)
            return false;

        if (oldValue == null && newValue == null) return true;
        if (oldValue == null || newValue == null) return false;

        if (!EqualityComparer<TValue>.Default.Equals(oldValue, newValue))
            return false;

        if (changeTrackingScope == ChangeTrackingScope.Instance)
            return true;

        if (newValue is IChangeTracking tracking && tracking.IsChanged)
        {
            if(changeTrackingScope == ChangeTrackingScope.InstanceProperty)
            {
                if (newValue is ICascadingChangeTracking cascading)
                    return !cascading.IsBaseChanged;
                return false;
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


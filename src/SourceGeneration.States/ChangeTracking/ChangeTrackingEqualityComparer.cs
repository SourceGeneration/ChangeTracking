using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace SourceGeneration.States;

public sealed class ChangeTrackingEqualityComparer : IEqualityComparer<object>
{
    public static readonly ChangeTrackingEqualityComparer Default = new();

    public new bool Equals(object? x, object? y)
    {
        if (!EqualityComparer<object>.Default.Equals(x, y)) return false;
        if (y is IChangeTracking tracking) return !tracking.IsChanged;
        return true;
    }

    public int GetHashCode([DisallowNull] object obj) => obj.GetHashCode();
}

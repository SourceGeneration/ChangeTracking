using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace SourceGeneration.ChangeTracking;

public sealed class ChangeTrackingSequenceEqualityComparer<TValue> : IEqualityComparer<TValue> where TValue : IEnumerable
{
    public static readonly ChangeTrackingSequenceEqualityComparer<TValue> Default = new();

    public bool Equals(TValue? x, TValue? y)
    {
        if (x == null && y == null) return true;
        if (x == null || y == null) return false;

        if (x is ICollection collection1 && y is ICollection collection2 && collection1.Count != collection2.Count)
            return false;

        var enumerator1 = ((IEnumerable)x).GetEnumerator();
        var enumerator2 = ((IEnumerable)y).GetEnumerator();

        var next1 = enumerator1.MoveNext();
        var next2 = enumerator2.MoveNext();
        while (next1 && next2)
        {
            var value2 = enumerator2.Current;
            if (value2 is IChangeTracking tracking)
            {
                if (tracking.IsChanged)
                    return false;
            }

            var value1 = enumerator1.Current;

            if (!EqualityComparer<object>.Default.Equals(value1, value2))
                return false;

            next1 = enumerator1.MoveNext();
            next2 = enumerator2.MoveNext();
        }

        return next1 == next2;
    }

    public int GetHashCode(TValue obj) => obj.GetHashCode();
}


using SourceGeneration.ChangeTracking;
using System.Collections.Generic;

namespace System.Linq;

public static class ChangeTrackingExtensions
{
    public static ChangeTrackingList<T> ToChangeTrackingList<T>(this IEnumerable<T> source) => new(source);
    public static ChangeTrackingDictionary<TKey, TValue> ToChangeTrackingList<TKey, TValue>(this IDictionary<TKey, TValue> source) where TKey : notnull => new(source);
}

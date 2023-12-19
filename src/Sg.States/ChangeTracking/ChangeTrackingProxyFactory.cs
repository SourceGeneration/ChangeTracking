using System.ComponentModel;

namespace SourceGeneration.States;

public static class ChangeTrackingProxyFactory
{
    private static readonly Dictionary<Type, Func<object, object>> _proxies = [];

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void Register<T>(Func<T, T> func) where T : class
    {
        _proxies.Add(typeof(T), x => func((T)x));
    }

    public static T Create<T>(T model)
    {
        if (model == null)
            return default!;

        if (_proxies.TryGetValue(typeof(T), out var func))
        {
            T value = (T)func(model!);

            if (value is IChangeTracking tracking)
            {
                tracking.AcceptChanges();
            }

            return value;
        }
        return model;
    }
}

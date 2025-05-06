using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace SourceGeneration.ChangeTracking;

public class ChangeTrackingDictionary<TKey, TValue> : ChangeTrackingObjectBase, IDictionary<TKey, TValue> where TKey : notnull
{
    private Dictionary<TKey, TValue> _dictionary = [];

    public ChangeTrackingDictionary() { }

    public ChangeTrackingDictionary(IDictionary<TKey, TValue>? dictionary)
    {
        if (dictionary != null)
        {
            foreach (var kvp in dictionary)
            {
                Add(kvp.Key, kvp.Value);
            }
        }
    }

    public TValue this[TKey key]
    {
        get => _dictionary[key];
        set
        {
            _dictionary.TryGetValue(key, out var original);
            if (!Equals(original, value))
            {
                RemoveNotifyEventSubscription(original);

                _dictionary[key] = value;
                AddNotifyEventSubscription(value);
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Replace,
                    new KeyValuePair<TKey, TValue>(key, value),
                    new KeyValuePair<TKey, TValue>(key, original!),
                    -1));
            }
        }
    }

    public void Add(TKey key, TValue value)
    {
        _dictionary.Add(key, value);
        _dictionary = new Dictionary<TKey, TValue>(_dictionary);
        AddNotifyEventSubscription(value);
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new KeyValuePair<TKey, TValue>(key, value)));
    }

    public void Add(KeyValuePair<TKey, TValue> item) => Add(item.Key, item.Value);

    public bool Remove(TKey key)
    {
        if (_dictionary.TryGetValue(key, out var value))
        {
            _dictionary.Remove(key);
            _dictionary = new Dictionary<TKey, TValue>(_dictionary);
            RemoveNotifyEventSubscription(value);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, new KeyValuePair<TKey, TValue>(key, value)));
            return true;
        }
        return false;
    }

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
        if (_dictionary.Remove(item.Key))
        {
            _dictionary = new Dictionary<TKey, TValue>(_dictionary);
            RemoveNotifyEventSubscription(item.Value);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
            return true;
        }
        return false;
    }

    public void Clear()
    {
        foreach (var item in _dictionary.Values)
        {
            RemoveNotifyEventSubscription(item);
        }

        _dictionary.Clear();
        _dictionary = [];
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }

    public ICollection<TKey> Keys => _dictionary.Keys;

    public ICollection<TValue> Values => _dictionary.Values;

    public int Count => _dictionary.Count;

    public bool IsReadOnly => false;

    public bool Contains(KeyValuePair<TKey, TValue> item) => ((IDictionary<TKey, TValue>)_dictionary).Contains(item);

    public bool ContainsKey(TKey key) => _dictionary.ContainsKey(key);

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => ((IDictionary<TKey, TValue>)_dictionary).CopyTo(array, arrayIndex);

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _dictionary.GetEnumerator();

#if NETCOREAPP3_0_OR_GREATER
    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value) => _dictionary.TryGetValue(key, out value);
#else
    public bool TryGetValue(TKey key, out TValue value) => _dictionary.TryGetValue(key, out value);
#endif

    IEnumerator IEnumerable.GetEnumerator() => _dictionary.GetEnumerator();

    public override void AcceptChanges()
    {
        _baseChanged = false;

        if (_cascadingChanged)
        {
            _cascadingChanged = false;

            foreach (var item in _dictionary.Values)
            {
                if (item is IChangeTracking tracking)
                {
                    tracking.AcceptChanges();
                }
            }
        }
    }
}
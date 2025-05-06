using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace SourceGeneration.ChangeTracking;

public class ChangeTrackingList<T> : ChangeTrackingObjectBase, IList<T>, IReadOnlyList<T>
{
    private readonly List<T> _list = [];

    public ChangeTrackingList() { }

    public ChangeTrackingList(IEnumerable<T>? list)
    {
        if (list != null)
        {
            AddRange(list);
        }
    }

    public T this[int index]
    {
        get => _list[index];
        set
        {
            var original = _list[index];
            if (!Equals(original, value))
            {

                RemoveNotifyEventSubscription(original);
                _list[index] = value;
                AddNotifyEventSubscription(value);
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, value, original, index));
            }
        }
    }

    public int Count => _list.Count;

    public bool IsReadOnly => false;

    public void Add(T item)
    {
        _list.Add(item);
        AddNotifyEventSubscription(item);
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
    }

    public void AddRange(IEnumerable<T> items)
    {
        foreach (var item in items)
        {
            _list.Add(item);
            AddNotifyEventSubscription(item);
        }
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, items));
    }

    public void Clear()
    {
        foreach (var item in _list)
        {
            RemoveNotifyEventSubscription(item);
        }

        _list.Clear();
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }

    public void Insert(int index, T item)
    {
        _list.Insert(index, item);
        AddNotifyEventSubscription(item);
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
    }

    public void InsertRange(int index, IEnumerable<T> items)
    {
        var array = items.ToArray();
        for (int i = 0; i < array.Length; i++)
        {
            T item = array[i];
            _list.Insert(i + index, item);
            AddNotifyEventSubscription(item);
        }
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, array, index));
    }

    public bool Remove(T item)
    {
        if (_list.Remove(item))
        {
            RemoveNotifyEventSubscription(item);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
            return true;
        }
        return false;
    }

    public void RemoveAt(int index)
    {
        var item = _list[index];
        RemoveNotifyEventSubscription(item);
        _list.RemoveAt(index);
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index));
    }

    public bool Contains(T item) => _list.Contains(item);
    public void CopyTo(T[] array, int arrayIndex) => _list.CopyTo(array, arrayIndex);
    public int IndexOf(T item) => _list.IndexOf(item);
    public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => _list.GetEnumerator();

    public override void AcceptChanges()
    {
        _baseChanged = false;

        if (_cascadingChanged)
        {
            _cascadingChanged = false;

            foreach (var item in _list)
            {
                if (item is IChangeTracking trackable)
                {
                    trackable.AcceptChanges();
                }
            }
        }
    }
}

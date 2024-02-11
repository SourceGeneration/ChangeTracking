using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;

namespace SourceGeneration.States;

public class ChangeTrackingList<T> : ChangeTrackingCollectionBase<T>, IList<T>
{
    private readonly List<T> _list = [];

    public ChangeTrackingList(IEnumerable<T>? list = null)
    {
        if (list != null)
        {
            foreach (var item in list)
            {
                Add(item);
            }
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
                TryRemoveNotifyEventSubscription(original);
                var newItem = ChangeTrackingProxyFactory.Create(value);
                _list[index] = newItem;
                TryAddNotifyEventSubscription(newItem);
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, newItem, original, index));
            }
        }
    }

    public int Count => ((ICollection<T>)_list).Count;

    public bool IsReadOnly => ((ICollection<T>)_list).IsReadOnly;

    public void Add(T item)
    {
        var newItem = ChangeTrackingProxyFactory.Create(item);
        _list.Add(newItem);
        TryAddNotifyEventSubscription(newItem);
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newItem));
    }

    public void AddRange(IEnumerable<T> items)
    {
        var newItems = items.Select(x => ChangeTrackingProxyFactory.Create(x)).ToList();
        foreach (var newItem in newItems)
        {
            _list.Add(newItem);
            TryAddNotifyEventSubscription(newItem);
        }
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newItems));
    }

    public void Clear()
    {
        foreach (var item in _list)
        {
            TryRemoveNotifyEventSubscription(item);
        }

        _list.Clear();
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }

    public void Insert(int index, T item)
    {
        var newItem = ChangeTrackingProxyFactory.Create(item);
        _list.Insert(index, newItem);
        TryAddNotifyEventSubscription(newItem);
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newItem, index));
    }

    public void InsertRange(int index, IEnumerable<T> items)
    {
        var newItems = items.Select(x => ChangeTrackingProxyFactory.Create(x)).ToList();
        for (int i = 0; i < newItems.Count; i++)
        {
            T newItem = newItems[i];
            _list.Insert(i + index, newItem);
            TryAddNotifyEventSubscription(newItem);
        }
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newItems, index));
    }

    public bool Remove(T item)
    {
        if (_list.Remove(item))
        {
            TryRemoveNotifyEventSubscription(item);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
            return true;
        }
        return false;
    }

    public void RemoveAt(int index)
    {
        var item = _list[index];
        TryRemoveNotifyEventSubscription(item);
        _list.RemoveAt(index);
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index));
    }

    public bool Contains(T item) => ((ICollection<T>)_list).Contains(item);
    public void CopyTo(T[] array, int arrayIndex) => ((ICollection<T>)_list).CopyTo(array, arrayIndex);
    public int IndexOf(T item) => ((IList<T>)_list).IndexOf(item);
    public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)_list).GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_list).GetEnumerator();

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

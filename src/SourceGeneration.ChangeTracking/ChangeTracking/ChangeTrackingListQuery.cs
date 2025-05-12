using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace SourceGeneration.ChangeTracking;

public interface IChangeTrackingListQuery<TCollection, TItem> :
    ICascadingChangeTracking, IDisposable, IEnumerable<TItem>
    where TCollection : IEnumerable<TItem>, INotifyCollectionChanged
{

}
internal class ChangeTrackingListQuery<TCollection, TItem> : IChangeTrackingListQuery<TCollection,TItem>
    where TCollection : IEnumerable<TItem>, INotifyCollectionChanged
{
    private readonly Func<TItem, bool> _predicate;
    private readonly TCollection _collection;

    private bool _baseChanged;
    private bool _cascadingChanged;
    public bool IsCascadingChanged => _cascadingChanged;
    public bool IsBaseChanged => _baseChanged;
    public bool IsChanged => _baseChanged || _cascadingChanged;

    public void AcceptChanges()
    {
        _cascadingChanged = false;
        _baseChanged = false;
    }

    public IEnumerator<TItem> GetEnumerator() => _collection.Where(_predicate).GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => _collection.Where(_predicate).GetEnumerator();

    public void Dispose()
    {
        _collection.CollectionChanged -= OnCollectionChanged;

        if (_collection is INotifyPropertyChanged propertyChanged)
        {
            propertyChanged.PropertyChanged -= OnItemPropertyChanged;
        }
    }

    public ChangeTrackingListQuery(TCollection collection, Func<TItem, bool> predicate)
    {
        _predicate = predicate;
        _collection = collection;

        _collection.CollectionChanged += OnCollectionChanged;

        if (_collection is INotifyPropertyChanged propertyChanged)
        {
            propertyChanged.PropertyChanged += OnItemPropertyChanged;
        }
    }

    private void OnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (sender is TItem item && _predicate(item))
        {
            _cascadingChanged = true;
        }
    }

    private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add && e.NewItems != null)
        {
            foreach (var item in e.NewItems)
            {
                if (item is TItem t && _predicate(t))
                {
                    _baseChanged = true;
                    return;
                }
            }
        }

        if (e.Action == NotifyCollectionChangedAction.Remove && e.OldItems != null)
        {
            foreach (var item in e.OldItems)
            {
                if (item is TItem t && _predicate(t))
                {
                    _baseChanged = true;
                    return;
                }
            }

        }
        if (e.Action == NotifyCollectionChangedAction.Reset && e.OldItems != null)
        {
            foreach (var item in e.OldItems)
            {
                if (item is TItem t && _predicate(t))
                {
                    _baseChanged = true;
                    return;
                }
            }
        }

        if (e.Action == NotifyCollectionChangedAction.Replace)
        {
            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems)
                {
                    if (item is TItem t && _predicate(t))
                    {
                        _baseChanged = true;
                        return;
                    }
                }
            }

            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    if (item is TItem t && _predicate(t))
                    {
                        _baseChanged = true;
                        return;
                    }
                }
            }
        }
    }

}

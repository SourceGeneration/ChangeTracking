using System.Collections.Specialized;
using System.ComponentModel;

namespace SourceGeneration.States;

public abstract class ChangeTrackingCollectionBase<T> : ICascadingChangeTracking, INotifyCollectionChanged, INotifyPropertyChanged
{
    protected bool _cascadingChanged;
    protected bool _baseChanged;

    public bool IsChanged => _cascadingChanged || _baseChanged;
    public bool IsCascadingChanged => _cascadingChanged;

    public event NotifyCollectionChangedEventHandler? CollectionChanged;
    public event PropertyChangedEventHandler? PropertyChanged;

    public abstract void AcceptChanges();

    protected void OnPropertyChanged(object? sender, PropertyChangedEventArgs args)
    {
        _cascadingChanged = true;
        PropertyChanged?.Invoke(sender, args);
    }

    protected void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs args)
    {
        _cascadingChanged = true;
        CollectionChanged?.Invoke(sender, args);
    }

    protected void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
    {
        _baseChanged = true;
        CollectionChanged?.Invoke(this, args);
    }

    protected void TryAddNotifyEventSubscription(T? item)
    {
        if (item is null)
        {
            return;
        }

        if (item is INotifyPropertyChanged notify)
        {
            notify.PropertyChanged += OnPropertyChanged;
        }

        if (item is INotifyCollectionChanged notify2)
        {
            notify2.CollectionChanged += OnCollectionChanged;
        }
    }

    protected void TryRemoveNotifyEventSubscription(T? item)
    {
        if (item is null)
        {
            return;
        }

        if (item is INotifyPropertyChanged notify)
        {
            notify.PropertyChanged -= OnPropertyChanged;
        }

        if (item is INotifyCollectionChanged notify2)
        {
            notify2.CollectionChanged -= OnCollectionChanged;
        }
    }
}

using System.Collections.Specialized;
using System.ComponentModel;

namespace SourceGeneration.ChangeTracking;

public abstract class ChangeTrackingCollectionBase<T> : ICascadingChangeTracking, INotifyCollectionChanged, INotifyPropertyChanging, INotifyPropertyChanged
{
    protected bool _cascadingChanged;
    protected bool _baseChanged;

    public bool IsChanged => _cascadingChanged || _baseChanged;
    public bool IsCascadingChanged => _cascadingChanged;

    public event PropertyChangingEventHandler? PropertyChanging;
    public event PropertyChangedEventHandler? PropertyChanged;
    public event NotifyCollectionChangedEventHandler? CollectionChanged;

    public abstract void AcceptChanges();

    protected void OnPropertyChanging(object? sender, PropertyChangingEventArgs args)
    {
        PropertyChanging?.Invoke(sender, args);
    }

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

        if (item is INotifyPropertyChanging notify1)
        {
            notify1.PropertyChanging += OnPropertyChanging;
        }

        if (item is INotifyPropertyChanged notify2)
        {
            notify2.PropertyChanged += OnPropertyChanged;
        }

        if (item is INotifyCollectionChanged notify3)
        {
            notify3.CollectionChanged += OnCollectionChanged;
        }
    }

    protected void TryRemoveNotifyEventSubscription(T? item)
    {
        if (item is null)
        {
            return;
        }

        if (item is INotifyPropertyChanging notify1)
        {
            notify1.PropertyChanging -= OnPropertyChanging;
        }

        if (item is INotifyPropertyChanged notify2)
        {
            notify2.PropertyChanged -= OnPropertyChanged;
        }

        if (item is INotifyCollectionChanged notify3)
        {
            notify3.CollectionChanged -= OnCollectionChanged;
        }
    }
}

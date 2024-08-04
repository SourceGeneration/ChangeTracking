using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace SourceGeneration.ChangeTracking;

public abstract class ChangeTrackingObjectBase : ICascadingChangeTracking, INotifyCollectionChanged, INotifyPropertyChanging, INotifyPropertyChanged
{
    protected bool _cascadingChanged;
    protected bool _baseChanged;

    public bool IsChanged => _cascadingChanged || _baseChanged;
    public bool IsCascadingChanged => _cascadingChanged;
    public bool IsBaseChanged => _baseChanged;

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
        if (_cascadingChanged) return;

        _cascadingChanged = true;
        PropertyChanged?.Invoke(sender, args);
    }

    protected void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs args)
    {
        if (_cascadingChanged) return;

        _cascadingChanged = true;
        CollectionChanged?.Invoke(sender, args);
    }

    protected void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
    {
        _baseChanged = true;

        if (args.NewItems != null)
        {
            if (_cascadingChanged == false && args.NewItems.OfType<IChangeTracking>().Any(x => x.IsChanged))
            {
                _cascadingChanged = true;
            }
        }

        CollectionChanged?.Invoke(this, args);
    }

    protected void AddNotifyEventSubscription(object? item)
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

    protected void RemoveNotifyEventSubscription(object? item)
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

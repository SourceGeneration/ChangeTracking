//namespace SourceGeneration.States.Test;

//[ChangeTracking]
//public partial class TrackingObject
//{
//    public TrackingObject()
//    {
//        IListOfValue = [];
//        IListOfObject = [];
//        IEnumerableOfValue = [];
//        IEnumerableOfObject = [];
//        ICollectionOfValue = [];
//        ICollectionOfObject = [];
//        IDictionaryOfValue = new Dictionary<int, int>();
//        IDictionaryOfObject = new Dictionary<int, TrackingObject>();
//        TrackingValueList = [];
//        TrackingObjectList = [];
//        TrackingValueDictionary = [];
//        TrackingObjectDictionary = [];
//        NotTracking = new();
//        CascadingTracking = new();
//        NotTrackingProperty = new();

//        AcceptChanges();
//    }

//    public string? StringProperty { get => GetStringProperty(); set => SetStringProperty(value); }
//    public int IntProperty { get => GetIntProperty(); set => SetIntProperty(value); }
//    public TrackingObject? CascadingObject { get => GetCascadingObject(); set => SetCascadingObject(value); }
//    public IList<int> IListOfValue { get => GetIListOfValue(); set => SetIListOfValue(value); }
//    public IList<TrackingObject> IListOfObject { get => GetIListOfObject(); set => SetIListOfObject(value); }
//    public IEnumerable<int> IEnumerableOfValue { get => GetIEnumerableOfValue(); set => SetIEnumerableOfValue(value); }
//    public IEnumerable<TrackingObject> IEnumerableOfObject { get => GetIEnumerableOfObject(); set => SetIEnumerableOfObject(value); }

//    public ICollection<int> ICollectionOfValue { get => GetICollectionOfValue(); set => SetICollectionOfValue(value); }
//    public ICollection<TrackingObject> ICollectionOfObject { get => GetICollectionOfObject(); set => SetICollectionOfObject(value); }

//    public IDictionary<int, int> IDictionaryOfValue { get => GetIDictionaryOfValue(); set => SetIDictionaryOfValue(value); }

//    public IDictionary<int, TrackingObject> IDictionaryOfObject { get => GetIDictionaryOfObject(); set => SetIDictionaryOfObject(value); }

//    public ChangeTrackingList<int> TrackingValueList { get => GetTrackingValueList(); set => SetTrackingValueList(value); }
//    public ChangeTrackingList<TrackingObject> TrackingObjectList { get => GetTrackingObjectList(); set => SetTrackingObjectList(value); }

//    public ChangeTrackingDictionary<int, int> TrackingValueDictionary { get => GetTrackingValueDictionary(); set => SetTrackingValueDictionary(value); }
//    public ChangeTrackingDictionary<int, TrackingObject> TrackingObjectDictionary { get => GetTrackingObjectDictionary(); set => SetTrackingObjectDictionary(value); }

//    public NotTrackingObject NotTracking { get => GetNotTracking(); set => SetNotTracking(value); }
//    public CascadingTrackingObject CascadingTracking { get => GetCascadingTracking(); set => SetCascadingTracking(value); }
//    public NotTrackingPropertyObject NotTrackingProperty { get => GetNotTrackingProperty(); set => SetNotTrackingProperty(value); }

//    public int ReadOnlyProperty { get; } = 1;
//    public virtual int ReadOnlyVirtualProperty { get; } = 1;
//}

//public class NotTrackingObject
//{
//    public virtual int Value { get; set; }
//}

//[ChangeTracking]
//public partial class NotTrackingPropertyObject
//{
//    public int Value { get => GetValue(); set => SetValue(value); }
//}

//[ChangeTracking]
//public partial class CascadingTrackingObject
//{
//    public virtual int Value { get => GetValue(); set => SetValue(value); }
//}

//[ChangeTracking]
//public class RequirePropertyTrackingObject
//{
//    public required virtual int Value { get; set; }
//    public required virtual int[] Array { get; set; } = [];
//    public required virtual IEnumerable<int> Enumerable { get; set; } = [];
//}
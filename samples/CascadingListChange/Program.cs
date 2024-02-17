using SourceGeneration.States;

State<Goods> state = new(new Goods());

// Bind Tags with scope `ChangeTrackingScope.Root`, it's default value
// The state will push last value when you first subscribe
// ouput: Tags count has changed 0
var disposable = state.Bind(
    selector: x => x.Tags, 
    subscriber: x => Console.WriteLine($"Tags count has changed: {x.Count}"), 
    scope: ChangeTrackingScope.Root);

// output: Tags count has changed: 1
state.Update(x => x.Tags.Add(new SubState { Tag = "first tag" }));

// no output, because Tags property is not changed
state.Update(x => x.Tags[0].Tag = "first tag has modified");

disposable.Dispose();

// Bind Tags with scope `ChangeTrackingScope.Cascading`
// The state will push last value when you first subscribe
// ouput: Tags value has changed: first tag has modified
state.Bind(
    selector: x => x.Tags,
    subscriber: x => Console.WriteLine($"Tags value has changed: {x[0].Tag}"),
    scope: ChangeTrackingScope.Cascading);

// ouput: Tags value has changed: first tag has modified * 2
state.Update(x => x.Tags[0].Tag = "first tag has modified * 2");


Console.ReadLine();

[ChangeTracking]
public class Goods
{
    public virtual int Number { get; set; }
    public virtual double Price { get; set; } = 30;
    public virtual int Count { get; set; }

    public virtual ChangeTrackingList<SubState> Tags { get; set; } = [];
}

[ChangeTracking]
public class SubState
{
    public virtual string? Tag { get; set; }
}
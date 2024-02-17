using SourceGeneration.States;

State<Goods> state = new(new Goods());

// Bind Records with scope `ChangeTrackingScope.Root`, it's default value
// The state will push last value when you first subscribe
// ouput SubState tas has changed: default
var disposable = state.Bind(
    selector: x => x.SubState,
    subscriber: x => Console.WriteLine($"SubState tas has changed: {x.Tag}"),
    scope: ChangeTrackingScope.Root);

// no output, because Tags property is not changed
state.Update(x => x.SubState.Tag = "a");

disposable.Dispose();

// Bind Records with scope `ChangeTrackingScope.Cascading`
// The state will push last value when you first subscribe
// ouput SubState tas has changed: a
state.Bind(
    selector: x => x.SubState,
    subscriber: x => Console.WriteLine($"SubState tas has changed: {x.Tag}"),
    scope: ChangeTrackingScope.Cascading);

// ouput SubState tas has changed: b
state.Update(x => x.SubState.Tag = "b");

Console.ReadLine();

[ChangeTracking]
public class Goods
{
    public virtual SubState SubState { get; set; } = new();
}

[ChangeTracking]
public class SubState
{
    public virtual string? Tag { get; set; } = "default";
}
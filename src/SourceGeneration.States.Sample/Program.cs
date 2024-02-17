// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using SourceGeneration.States;

State<List<int>> state2 = new([1, 2, 3]);

state2.Bind(x => x, x => Console.WriteLine("list changed"), ChangeTrackingSequenceEqualityComparer<List<int>>.Default);

state2.Update(x =>
{
    x[0] = -1;
});

var services = new ServiceCollection().AddState<MyState>().BuildServiceProvider();

var state = services.GetRequiredService<State<MyState>>();

//state.Bind(x => x.A, x => Console.WriteLine("Property A has changed: " + x));
state.Bind(x => x.B, x => Console.WriteLine("Property B has changed: " + x));
state.Bind(x => x, x => Console.WriteLine("SubState has changed: " + x.SubState.Number), ChangeTrackingScope.Cascading);
state.Bind(x => x.List, x => Console.WriteLine("Property List has changed"), ChangeTrackingScope.Root);

state.SubscribeBindingChanged(_ => Console.WriteLine("One or many bindings has changed"));

//state.Update(x => x.A = 1);
//state.Update(x => x.A = 1);

//state.Update(x => x.B = "a");
//state.Update(x => x.B = "a");

//state.Update(x => x.C = "a");

//state.Update(x => x.List = [1]);
//state.Update(x => x.List!.Add(2));
state.Update(x => x.SubState.Number = 3);
//state.Update(x => x.B = "b");

Console.ReadLine();

[ChangeTracking]
public class MyState
{
    public virtual int A { get; set; }
    public virtual string? B { get; set; }
    public virtual string? C { get; set; }

    public virtual ChangeTrackingList<int> List { get; set; } = [];

    public virtual MyOtherState SubState { get; set; } = new();
}

[ChangeTracking]
public class MyOtherState
{
    public virtual int Number { get; set; }
}
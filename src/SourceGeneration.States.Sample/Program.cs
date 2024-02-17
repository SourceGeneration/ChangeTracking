// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using SourceGeneration.States;

var services = new ServiceCollection().AddState<MyState>().BuildServiceProvider();

var state = services.GetRequiredService<IStore<MyState>>();

//state.Bind(x => x.A, x => Console.WriteLine("Property A has changed: " + x));
//state.Bind(x => x.B, x => Console.WriteLine("Property B has changed: " + x));
state.Bind(x => x.List, x => Console.WriteLine("Property List has changed: " + x), ChangeTrackingScope.RootOrCascadingChanged);

state.SubscribeBindingChanged(_ => Console.WriteLine("Binding property A or B has changed"));

//state.Update(x => x.A = 1);
//state.Update(x => x.A = 1);

//state.Update(x => x.B = "a");
//state.Update(x => x.B = "a");

//state.Update(x => x.C = "a");

//state.Update(x => x.List = [1]);
state.Update(x => x.List!.Add(2));
//state.Update(x => x.B = "b");

Console.ReadLine();

[ChangeTracking]
public class MyState
{
    public virtual int A { get; set; }
    public virtual string? B { get; set; }
    public virtual string? C { get; set; }

    public virtual ChangeTrackingList<int> List { get; set; } = [];
}
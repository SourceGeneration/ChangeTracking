// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using SourceGeneration.States;

var services = new ServiceCollection().AddState<MyState>().BuildServiceProvider();

var state = services.GetRequiredService<IStore<MyState>>();

state.Bind(x => x.A, x => Console.WriteLine(x));

state.Update(x =>
{
    x.A = 2;
});
state.Update(x =>
{
    x.A = 2;
});
state.Update(x =>
{
    x.A = 3;
});

Console.ReadLine();

[ChangeTracking]
public class MyState
{
    public virtual int A { get; set; }
    public virtual string B { get; set; }
}
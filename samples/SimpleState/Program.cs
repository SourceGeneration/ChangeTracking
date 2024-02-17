using SourceGeneration.States;

State<Goods> state = new(new Goods());

var disposable = state.Bind(x => x.Price, x => Console.WriteLine($"Price has changed: {x}"));
state.Bind(x => x.Count, x => Console.WriteLine($"Count has changed: {x}"));

// ouput Price changed: 3.14
// ouput Price changed: 3
state.Update(x =>
{
    x.Price = 3.14;
    x.Count = 3;
});

// no ouput, the value was has changed
state.Update(x => x.Price = 3.14);

// no ouput, because the property of Number was not subscribe
state.Update(x => x.Number = 1);

// Unsubscribe price property binding 
disposable.Dispose();

// no ouput, the value was has changed
state.Update(x => x.Price = 4.28);

Console.ReadLine();

[ChangeTracking]
public class Goods
{
    public virtual int Number { get; set; }
    public virtual double Price { get; set; }
    public virtual int Count { get; set; }
}
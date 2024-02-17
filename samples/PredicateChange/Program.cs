using SourceGeneration.States;

State<Goods> state = new(new Goods());

// bind property Price with condition : price >= 10
state.Bind(
    selector: x => x.Price,
    predicate: x => x >= 10,
    subscriber: x => Console.WriteLine($"Price changed: {x}"));

// no console ouput, the value is less than 10
state.Update(x => x.Price = 9);

// ouput Price changed: 10
state.Update(x => x.Price = 10);

Console.ReadLine();

[ChangeTracking]
public class Goods
{
    public virtual int Number { get; set; }
    public virtual double Price { get; set; }
    public virtual int Count { get; set; }
}
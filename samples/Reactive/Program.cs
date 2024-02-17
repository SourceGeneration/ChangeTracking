using SourceGeneration.States;
using System.Reactive.Linq;

State<Goods> state = new(new Goods
{
    Count = 5,
});

// The state will push last value when you subscribed
// ouput: 5
state
    .Where(x => x.Count >= 5)
    .Select(x => x.Count)
    .DistinctUntilChanged()
    .Subscribe(x => Console.WriteLine(x));

// no ouput
state.Update(x => x.Count = 2);

// ouput 10
state.Update(x => x.Count = 10);

Console.ReadLine();

[ChangeTracking]
public class Goods
{
    public virtual int Count { get; set; }
}
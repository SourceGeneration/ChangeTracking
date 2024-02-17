using SourceGeneration.States;

int count = 0;
double price = 0;
State<Goods> state = new(new Goods());

state.Bind(x => x.Count, x => count = x);
state.Bind(x => x.Price, x => price = x);

state.SubscribeBindingChanged(state =>
{
    Console.WriteLine($"Count or Price has changed. Count={count}, Price={state.Price}");
});

//ouput: Count or Price has changed
state.Update(x =>
{
    x.Price = 3.14;
    x.Count = 10;
});

//no output, because Count has not changed
state.Update(x => x.Count = 10);

//no output, because property Number has not bound 
state.Update(x => x.Number = 3);

//ouput: Count or Price has changed
state.Update(x => x.Count = 11);

Console.ReadLine();

[ChangeTracking]
public class Goods
{
    public virtual int Number { get; set; }
    public virtual double Price { get; set; }
    public virtual int Count { get; set; }
}
// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using SourceGeneration.States;

var services = new ServiceCollection()
    .AddState<GoodsState>(ServiceLifetime.Singleton)
    .AddState<CustomerState>(ServiceLifetime.Singleton)
    .BuildServiceProvider();

var goods = services.GetRequiredService<State<GoodsState>>();
var customer = services.GetRequiredService<State<CustomerState>>();

goods.Bind(x => x, x => x.Count > 10, x => Console.WriteLine(x.Count));
customer.Bind(x => x, x => Console.WriteLine(x.Name));

goods.Update(x => x.Count = 13);
customer.Update(x => x.Name = "abc");

Console.ReadLine();

[ChangeTracking]
public class GoodsState
{
    public virtual double Price { get; set; }
    public virtual int Count { get; set; }
}

[ChangeTracking]
public class CustomerState
{
    public virtual int Number { get; set; }
    public virtual string? Name { get; set; } = "default";
}
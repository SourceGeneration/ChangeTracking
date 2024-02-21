# States

[![NuGet](https://img.shields.io/nuget/vpre/SourceGeneration.States.svg)](https://www.nuget.org/packages/SourceGeneration.States)

States is a state management framework based on Source Generator and Reactive (RX) with no emissions, and it supports AOT compilation.

## Installing States

```powershell
Install-Package SourceGeneration.States -Version 1.0.0-beta2.240221.2
```

```powershell
dotnet add package SourceGeneration.States --version 1.0.0-beta2.240221.2
```

## ChangeTacking

States source generator will generate proxy class for your state type, you just need to add `ChangeTrackingAttriute`, property must be `virtual` and have a setter

```c#
[ChangeTracking]
public class Goods
{
    public virtual int Number { get; set; }
    public virtual double Price { get; set; }
    public virtual int Count { get; set; }
}
```

The proxy class implement `INotifyPropertyChanged` and `IChangeTracking`

```c#
internal class Goods__Proxy__ : Goods, INotifyPropertyChanged, System.ComponentModel.IChangeTracking
{
    //Properties override
}
```

States determines whether an object has been modified through two methods:
- Checking if the object reference has changed.
- Checking IChangeTracking.IsChanged property.


## Subscribe

```c#
State<Goods> state = new(new Goods());

state.Bind(x => x.Price, x => Console.WriteLine($"Price has changed: {x}"));
state.Bind(x => x.Count, x => Console.WriteLine($"Count has changed: {x}"));

// ouput Price changed: 3.14
// ouput Price changed: 3
state.Update(x =>
{
    x.Price = 3.14;
    x.Count = 3;
});

// no ouput, the value has not changed
state.Update(x => x.Price = 3.14);

// no ouput, because the property of Number was not subscribe
state.Update(x => x.Number = 1);
```

## Predicate

```c#
state.Bind(
    selector: x => x.Price,
    predicate: x => x >= 10,
    subscriber: x => Console.WriteLine($"Price changed: {x}"));

// no console ouput, the value is less than 10
state.Update(x => x.Price = 9);

// ouput Price changed: 10
state.Update(x => x.Price = 10);
```

## Change Scope
States support change scope, You can specify the scope of the subscribed changes.

- **ChangeTrackingScope.Root** `default value`  
  The subscription only be triggered when there are changes in the properties of the object itself.
- **ChangeTrackingScope.Cascading**  
  The subscription will be triggered when there are changes in the properties of the object itself or in the properties of its property objects.
- **ChangeTrackingScope.Always**  
  The subscription will be triggered whenever the `Update` method is called, regardless of whether the value has changed or not.

```c#
[ChangeTracking]
public class Goods
{
    public virtual ChangeTrackingList<SubState> Tags { get; set; } = [];
}

[ChangeTracking]
public class SubState
{
    public virtual string? Tag { get; set; }
}
```

```c#
// Bind Tags with scope `ChangeTrackingScope.Root`, it's default value
// The state will push last value when you subscribed
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
// The state will push last value when you subscribed
// ouput: Tags value has changed: first tag has modified
state.Bind(
    selector: x => x.Tags,
    subscriber: x => Console.WriteLine($"Tags value has changed: {x[0].Tag}"),
    scope: ChangeTrackingScope.Cascading);

// ouput: Tags value has changed: first tag has modified * 2
state.Update(x => x.Tags[0].Tag = "first tag has modified * 2");
```

## Reactive(Rx) Supports
State implement `IObserable<>`, so you can use Rx framework like `System.Reactive`,  
*Note: States does not have a dependency on System.Reactive.*

```c#
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
```

## Merge Changes

Some times we need to merge all changes, 
you can use `SubscribeBindingChanged`

```c#
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

//no output, because property Number has not subscribed 
state.Update(x => x.Number = 3);

//ouput: Count or Price has changed
state.Update(x => x.Count = 11);
```

## DependencyInjection

Using `AddState` to inject state, `ServiceLifetime.Scoped` is default value
```c#
var services = new ServiceCollection()
    .AddState<GoodsState>(ServiceLifetime.Singleton)
    .AddState<CustomerState>(ServiceLifetime.Singleton)
    .BuildServiceProvider();

var state = services.GetRequiredService<State<GoodsState>>();    
```

Using `StateInjectAttribute` to inject state,
`Source Generator` generated.

```c#
var services = new ServiceCollection().AddStateInjection().BuildServiceProvider();
var state = services.GetRequiredService<State<GoodsState>>();

[StateInject(ServiceLifetime.Singleton)]
[ChangeTracking]
public class GoodsState
{
    public virtual double Price { get; set; }
    public virtual int Count { get; set; }
}
```

## Dispose & State Scope

In most usage scenarios, when your page or component subscribes to the state, it must explicitly unsubscribe when the component is destroyed, otherwise it will result in a significant resource consumption.

```c#
State<Goods> state = new(new Goods());

var disposable1 = state.Bind(x => x, x => {});
var disposable2 = state.Bind(x => x, x => {});
var disposable3 = state.SubscribeBindingChanged(() => { });

disposable1.Dispose();
disposable2.Dispose();
disposable3.Dispose();
```

Of course, you can directly destroy the State object,
However, in most cases, 
the lifecycle of `State` needs to be consistent with the user session lifecycle,
so directly destroying `State` does not align with the application scenario.

```c#
state.Dispose();
```

To facilitate management, you can create an `IScopedState` by calling the `CreateScope` method.
In dependency injection, whether it is `ServiceLifetime.Singleton` or `ServiceLifetime.Scoped`, IScopedState is always `Transient`. `IScopedState` is more like a state view.

```c#
State<Goods> state = new(new Goods());
Assert.IsTrue(state.IsRoot);

IScopedState<GoodsState> scopedState = State.CreateScope();
Assert.IsFalse(scoped.IsRoot);
// bind or update

scopedState.Dispose();
```

## Blazor

You can use `States` in `Blazor`, it supports `AOT` compilation

**WebAssembly or Hybird**

```c#
services.AddState<GoodsState>(ServiceLifetime.Singleton);
```

**Server**

```c#
services.AddState<GoodsState>(ServiceLifetime.Scoped);
```

```c#
@inject IScopedState<MyState> State
@implements IDisposable

<h1>Count: @Count</h1>
<button @onclick="Click">Add</button>

@code{
    private int Count;

    protected override void OnInitialized()
    {
        State.Bind(x => x.Count, x => Count = x);
        State.SubscribeBindingChanged(StateHasChanged);
    }

    private void Click()
    {
        State.Update(x => x.Count++);
    }

    public void Dispose()
    {
        State.Dispose();
    }
}
```

You can use the Blux library to simplify this process, more information see [**Blux**](https://github.com/SourceGeneration/Blux) repo

```c#
@inherits BluxComponentBase
@inject IScopedState<MyState> State

<h1>Count: @Count</h1>
<button @onclick="Click">Add</button>

@code{
    private int Count;

    protected override void OnStateBinding()
    {
        State.Bind(x => x.Count, x => Count = x);
    }

    private void Click()
    {
        State.Update(x => x.Count++);
    }
}

```
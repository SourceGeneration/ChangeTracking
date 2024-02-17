using BlazorStates.Client.States;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SourceGeneration.States;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddState<MyState>(ServiceLifetime.Singleton);
await builder.Build().RunAsync();
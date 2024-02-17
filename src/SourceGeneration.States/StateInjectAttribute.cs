using Microsoft.Extensions.DependencyInjection;

namespace SourceGeneration.States;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class StateInjectAttribute(ServiceLifetime serviceLifetime = ServiceLifetime.Scoped) : Attribute
{
    public ServiceLifetime ServiceLifetime { get; } = serviceLifetime;
}
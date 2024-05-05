using Microsoft.Extensions.DependencyInjection;

namespace SourceGeneration.States.Test;

[TestClass]
public class InjectionTest
{
    [TestMethod]
    public void Test()
    {
        ServiceCollection services = new();
        services.AddState<TrackingObject>();

        var provider = services.BuildServiceProvider();

        var a = provider.GetRequiredService<State<TrackingObject>>();
        var b = provider.GetRequiredService<TrackingObject>();

        Assert.AreEqual(a.Value, b);
    }

    
}

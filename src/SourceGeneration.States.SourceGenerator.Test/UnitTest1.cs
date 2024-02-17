using Microsoft.Extensions.DependencyInjection;

namespace SourceGeneration.States.SourceGenerator.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            string source = @"
using SourceGeneration.States;
using Microsoft.Extensions.DependencyInjection;

[StateInject(ServiceLifetime.Transient)]
public class TestModel
{
    public required virtual int[] Array { get; set; } = [];
}
";
            var result = CSharpTestGenerator.Generate<StateInjectSourceGenerator>(source, typeof(ChangeTrackingAttribute).Assembly, typeof(ServiceLifetime).Assembly);
        }
    }
}
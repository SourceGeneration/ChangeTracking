namespace SourceGeneration.ChangeTracking.SourceGenerator.Test;

[TestClass]
public partial class UnitTest1
{
    [TestMethod]
    public void TestMethod1()
    {
        string source = @"
using SourceGeneration.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

[ChangeTracking]
public partial class CascadingTestObject
{
    public partial int ReadOnlyProperty { get; }
}

";
        var result = CSharpTestGenerator.Generate<ChanageTrackingSourceGenerator>(source, typeof(ChangeTrackingAttribute).Assembly);
        var s = result.RunResult.Results.FirstOrDefault().GeneratedSources.FirstOrDefault().SourceText?.ToString();
    }
}

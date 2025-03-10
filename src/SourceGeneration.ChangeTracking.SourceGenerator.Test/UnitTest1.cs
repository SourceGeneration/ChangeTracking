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
public partial class GenericType<T>
{
    //public partial int A { get; set; } = 1;
    public partial ChangeTrackingList<int> B { get; } 
    //public new partial int NewProperty { get; set; } = 1;
}


";
        var result = CSharpTestGenerator.Generate<ChanageTrackingSourceGenerator>(source, typeof(ChangeTrackingAttribute).Assembly);
        var s = result.RunResult.Results.FirstOrDefault().GeneratedSources.FirstOrDefault().SourceText?.ToString();
    }
}

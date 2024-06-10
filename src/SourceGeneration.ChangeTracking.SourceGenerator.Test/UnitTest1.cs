namespace SourceGeneration.ChangeTracking.SourceGenerator.Test;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void TestMethod1()
    {
        string source = @"
using SourceGeneration.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

[ChangeTracking]
public partial class TestModel
{
    public partial string A { get; set; }
    public partial int B { get; }
    public partial bool? C { get; init; }
    public partial int[] D { get; set; }
    public partial object E { get; set; }
    public partial IList<object> F { get; set; }
    public partial TestModel2 G {get;set;}
}

[ChangeTracking]
public partial class TestModel2
{
    public partial string A { get; set; }
    public partial int B { get; }
}

[ChangeTracking]
public partial class TestModel3 : TestModel2
{
    public partial string C { get; set; }
    public partial int D { get; }
}

";
        var result = CSharpTestGenerator.Generate<ChanageTrackingSourceGenerator>(source, typeof(ChangeTrackingAttribute).Assembly);
        var s = result.RunResult.Results.FirstOrDefault().GeneratedSources.FirstOrDefault().SourceText?.ToString();
    }
}
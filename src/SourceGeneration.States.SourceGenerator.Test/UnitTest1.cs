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

[ChangeTracking]
public class TestModel
{
    public required virtual int[] Array { get; set; } = [];
}
";
            CSharpTestGenerator.Generate<ChanageTrackingProxySourceGenerator>(source, typeof(ChangeTrackingAttribute).Assembly);
        }

//        [TestMethod]
//        public void TestMethod2()
//        {
//            string source = @"
//using SourceGeneration.States;

//[ChangeTracking]
//public partial class Obj1
//{
//    public string Text
//    {
//        get => GetText();
//        set => SetText(value);
//    }
//}
//";
//            CSharpTestGenerator.Generate<ChanageTrackingPartialSourceGenerator>(source, typeof(ChangeTrackingAttribute).Assembly);
//        }

    }
}
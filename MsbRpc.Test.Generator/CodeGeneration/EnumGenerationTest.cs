using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Generator;
using MsbRpc.Test.Generator.Base;

namespace MsbRpc.Test.Generator.CodeGeneration;

[TestClass]
public class EnumGenerationTest : ContractGenerationTest<EnumGenerationTest, ContractGenerator>
{
    private const string Code = @"internal enum MyEnum : byte
{
    Call,
    Return
}

[RpcContract]
internal interface IEnumTest : IRpcContract
{
    MyEnum Call(MyEnum value);
}";

    private const string Namespace = nameof(EnumGenerationTest);

    public EnumGenerationTest() : base(Code, Namespace) { }

    [TestMethod]
    public async Task GeneratorHasResult()
    {
        await TestGeneratorHasResult();
    }

    [TestMethod]
    public async Task GeneratorThrowsNoException()
    {
        await TestGeneratorThrowsNoException();
    }

    [TestMethod]
    public async Task GeneratorReportsNoDiagnostics()
    {
        await TestGeneratorReportsNoDiagnostics();
    }

    [TestMethod]
    public async Task FinalCompilationReportsNoDiagnostics()
    {
        await TestFinalCompilationReportsNoDiagnostics();
    }

    [TestMethod]
    public async Task GeneratesAnyTrees()
    {
        await TestGeneratesAnyTrees();
    }

    [TestMethod]
    public async Task GeneratesServerEndPoint()
    {
        await TestGeneratesFile("EnumTestServerEndPoint.g.cs");
    }

    [TestMethod]
    public async Task GeneratesClientEndPoint()
    {
        await TestGeneratesFile("EnumTestClientEndPoint.g.cs");
    }
}
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Generator;
using MsbRpc.Test.Generator.Base;

namespace MsbRpc.Test.Generator.CodeGeneration;

[TestClass]
public class ArrayElementCounterTest : ContractGenerationTest<ArrayElementCounterTest, ContractGenerator>
{
    private const string Code = @"[RpcContract(RpcContractDirection.ClientToServer)]
public interface IArrayElementCounter : IRpcContract
{
    int CountElements(int[] array);
}";

    private const string Namespace = nameof(ArrayElementCounterTest);

    public ArrayElementCounterTest() : base(Code, Namespace) { }

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
        await TestGeneratesFile("ArrayElementCounterServerEndPoint.g.cs");
    }

    [TestMethod]
    public async Task GeneratesClientEndPoint()
    {
        await TestGeneratesFile("ArrayElementCounterClientEndPoint.g.cs");
    }
}
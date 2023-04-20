using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Generator;

namespace MsbRpc.Test.Generator.Echo.Tests;

[TestClass]
public class GeneratorTest : ContractGenerationTest<GeneratorTest, ContractGenerator>
{
    private const string Code = @"[RpcContract(RpcContractType.ClientToServer)]
public interface IEcho : IRpcContract
{
    System.DateTime GetDateTime();
}";

    private const string Namespace = "MsbRpc.Test.Generator.Echo.Tests";

    public GeneratorTest() : base(Code, Namespace) { }

    [TestMethod]
    public async Task GeneratorRuns()
    {
        await TestGeneratorRuns();
    }

    [TestMethod]
    public async Task GeneratorHasOneResult()
    {
        await TestGeneratorHasOneResult();
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
}
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Generator;
using MsbRpc.Test.Generator.Base;

namespace MsbRpc.Test.Generator.CodeGeneration;

[TestClass]
[TestCategory(TestCategories.Contract)]
public class EmptyContractGenerationTest : ContractGenerationTest<EmptyContractGenerationTest, ContractGenerator>
{
    private const string Code = @"[RpcContract, GenerateServer]
public interface IEmptyContract : IRpcContract { }";

    private const string Namespace = nameof(EmptyContractGenerationTest);

    private const string ContractName = "EmptyContract";

    public EmptyContractGenerationTest() : base(Code, Namespace, ContractName) { }

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
    public async Task GeneratesAnyTrees() => await TestGeneratesAnyTrees();

    #region FileGeneration

    [TestMethod]
    public async Task GeneratesServerEndPoint() => await TestGeneratesServerEndPoint();

    [TestMethod]
    public async Task GeneratesClientEndPoint() => await TestGeneratesClientEndPoint();

    [TestMethod]
    public async Task GeneratesClientEndPointConfigurationBuilder() => await TestGeneratesClientEndPointConfigurationBuilder();

    [TestMethod]
    public async Task GeneratesServerEndPointConfigurationBuilder() => await TestGeneratesServerEndPointConfigurationBuilder();

    [TestMethod]
    public async Task GeneratesServerConfigurationBuilder() => await TestGeneratesServerConfigurationBuilder();

    [TestMethod]
    public async Task GeneratesServer() => await TestGeneratesServer();

    #endregion
}
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Generator;

namespace MsbRpc.Test.Generator;

[TestClass]
public class UnresolvedDateTimeTypeTest : ContractGenerationTest<UnresolvedDateTimeTypeTest, ContractGenerator>
{
    private const string Code = @"[RpcContract(RpcContractType.ClientToServer)]
public interface IEcho : IRpcContract
{
    System.DateTime GetDateTime(System.DateTime myDateTime);
}";

    private const string Namespace = "MsbRpc.Test.Generator.Echo.Tests";

    public UnresolvedDateTimeTypeTest() : base(Code, Namespace) { }

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
    public async Task GeneratorReportsUnresolvedReturnTypeDiagnostic()
    {
        await TestGeneratorReportsDiagnostics(DiagnosticDescriptors.TypeIsNotAValidRpcReturnType.Id);
    }

    [TestMethod]
    public async Task GeneratorReportsUnresolvedParameterTypeDiagnostic()
    {
        await TestGeneratorReportsDiagnostics(DiagnosticDescriptors.TypeIsNotAValidRpcParameter.Id);
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
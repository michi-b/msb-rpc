#region

using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Generator;
using MsbRpc.Test.Generator.Base;

#endregion

namespace MsbRpc.Test.Generator.CodeGeneration;

[TestClass]
[TestCategory(TestCategories.Contract)]
public class UnresolvedDateTimeTypeTest : ContractGenerationTest<UnresolvedDateTimeTypeTest, ContractGenerator>
{
    private const string Code = @"[RpcContract]
public interface IDateTimeEcho : IRpcContract
{
    System.DateTime GetDateTime(System.DateTime myDateTime);
}";

    private const string Namespace = nameof(UnresolvedDateTimeTypeTest);
    private const string ContractName = "DateTimeEcho";

    public UnresolvedDateTimeTypeTest() : base(Code, Namespace, ContractName) { }

    [TestMethod]
    public async Task GeneratorRuns()
    {
        await TestGeneratorRuns();
    }

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
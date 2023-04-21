using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Misbat.CodeAnalysis.Test.CodeTest;
using MsbRpc.Generator;

namespace MsbRpc.Test.Generator;

[TestClass]
public class UnresolvedDateTimeTypeTest : ContractGenerationTest<UnresolvedDateTimeTypeTest, ContractGenerator>
{
    private const string Code = @"[RpcContract(RpcContractType.ClientToServer)]
public interface IDateTimeEcho : IRpcContract
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
    public async Task GeneratorHasResult()
    {
        CodeTestResult result = await TestGeneratorHasResult(LoggingOptions.None);
        var stringBuilder = new StringBuilder(10000);

        // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
        // LINQ would be less readable here
        foreach (SyntaxTree tree in result.GetGeneratorDriverRunResult<ContractGenerator>().GeneratedTrees)
        {
            foreach (TextLine line in (await tree.GetTextAsync()).Lines)
            {
                stringBuilder.AppendLine(line.ToString());
            }
        }

        Logger.LogInformation("Generated trees:\n{Trees}", stringBuilder.ToString());
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
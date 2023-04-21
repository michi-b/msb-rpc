using System.IO;
using System.Linq;
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
public class IncrementerGenerationTest : ContractGenerationTest<IncrementerGenerationTest, ContractGenerator>
{
    private const string Code = @"[RpcContract(RpcContractType.ClientToServer)]
public interface IIncrementer : IRpcContract
{
    int Increment(int value);
    int? IncrementNullable(int? value);
    string IncrementString(string value);
    public void Store(int value);
    public void IncrementStored();
    public int GetStored();
    public void Finish();
}";

    private const string Namespace = "MsbRpc.Test.Serialization.ManualRpcTest.Incrementer.Input";

    public IncrementerGenerationTest()
        : base(Code, Namespace) { }

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
    public async Task GeneratesServerProcedureEnum()
    {
        await TestGeneratesFile("IncrementerProcedure.g.cs");
    }

    [TestMethod]
    public async Task GeneratesServerProcedureEnumExtensions()
    {
        await TestGeneratesFile("IncrementerProcedureExtensions.g.cs");
    }

    [TestMethod]
    public async Task GeneratesServerEndPoint()
    {
        await TestGeneratesFile("IncrementerServerEndPoint.g.cs");
    }

    [TestMethod]
    public async Task GeneratesClientEndPoint()
    {
        await TestGeneratesFile("IncrementerClientEndPoint.g.cs");
    }
}
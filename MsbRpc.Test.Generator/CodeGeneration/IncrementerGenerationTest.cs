using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Generator;
using MsbRpc.Test.Generator.Base;

namespace MsbRpc.Test.Generator.CodeGeneration;

[TestClass]
[TestCategory(TestCategories.Contract)]
public class IncrementerGenerationTest : ContractGenerationTest<IncrementerGenerationTest, ContractGenerator>
{
    private const string Code = @"[RpcContract]
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

    private const string Namespace = nameof(IncrementerGenerationTest);

    private const string ContractName = "Incrementer";

    public IncrementerGenerationTest()
        : base(Code, Namespace, ContractName) { }

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
        await TestGeneratesProcedureEnum();
    }

    [TestMethod]
    public async Task GeneratesServerProcedureEnumExtensions()
    {
        await TestGeneratesProcedureEnumExtensions();
    }

    [TestMethod]
    public async Task GeneratesServerEndPoint()
    {
        await TestGeneratesServerEndPoint();
    }

    [TestMethod]
    public async Task GeneratesClientEndPoint()
    {
        await TestGeneratesClientEndPoint();
    }
}
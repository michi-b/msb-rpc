using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Generator;
using MsbRpc.Test.Generator.Base;

namespace MsbRpc.Test.Generator.CodeGeneration;

[TestClass]
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
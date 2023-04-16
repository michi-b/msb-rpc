using System.Collections.Immutable;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Misbat.CodeAnalysis.Test.CodeTest;

namespace MsbRpc.Test.Generator.Echo.Tests;

[TestClass]
public class GeneratorTest : SingleSourceGeneratorTest<GeneratorTest>
{
    protected override string Code
        => @"[RpcContract(RpcContractType.ClientToServer)]
public interface IEcho : IRpcContract
{
    System.DateTime GetDateTime();
}";

    protected override string Namespace => "MsbRpc.Test.Generator.Echo.Tests";

    [TestMethod]
    public async Task FinalCompilationReportsNoDiagnostics()
    {
        CodeTestResult result = await RunCodeTest(CodeTest.LoggingOptions.FinalDiagnostics);
        ImmutableArray<Diagnostic> diagnostics = result.Compilation.GetDiagnostics();
        Assert.AreEqual(0, diagnostics.Length);
    }
}
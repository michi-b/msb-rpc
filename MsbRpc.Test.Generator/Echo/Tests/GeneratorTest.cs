using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Contracts;
using MsbRpc.Generator;
using MsbRpc.Generator.Attributes;

namespace MsbRpc.Test.Generator.Echo.Tests;

[TestClass]
public class GeneratorTest : SingleSourceGeneratorTest<GeneratorTest, ContractGenerator>
{
    protected override string Code
        => @"[RpcContract(RpcContractType.ClientToServer)]
public interface IEcho : IRpcContract
{
    System.DateTime GetDateTime();
}";

    protected override string Namespace => "MsbRpc.Test.Generator.Echo.Tests";

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
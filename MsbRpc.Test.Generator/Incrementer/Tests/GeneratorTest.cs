﻿using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Generator;

namespace MsbRpc.Test.Generator.Incrementer.Tests;

[TestClass]
public class GeneratorTest : ContractGenerationTest<GeneratorTest, ContractGenerator>
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

    public GeneratorTest()
        : base(Code, Namespace) { }

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
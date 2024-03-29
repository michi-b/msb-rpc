﻿#region

using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Generator;
using MsbRpc.Test.Generator.Base;

#endregion

namespace MsbRpc.Test.Generator.CodeGeneration;

[TestClass]
[TestCategory(TestCategories.Contract)]
public class ArrayElementCounterTest : ContractGenerationTest<ArrayElementCounterTest, ContractGenerator>
{
    private const string Code = @"[RpcContract]
public interface IArrayElementCounter : IRpcContract
{
    int CountElements(int[] array);
}";

    private const string Namespace = nameof(ArrayElementCounterTest);

    private const string ContractName = "ArrayElementCounter";

    public ArrayElementCounterTest() : base(Code, Namespace, ContractName) { }

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

    #endregion
}
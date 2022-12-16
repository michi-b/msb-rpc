using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Misbat.CodeAnalysis.Test.CodeTest;
using Misbat.CodeAnalysis.Test.Extensions;
using Misbat.CodeAnalysis.Test.Utility;
using MsbRpc.Generator;
using MsbRpc.Generator.Attributes;

namespace MsbRpcTest.ManualRpcTest.Incrementer.Tests;

[TestClass]
public class GeneratorTest : Test
{
    private const string Code = @"[RpcContract]
public interface IIncrementer
{
    int Increment(int value);
}";

    private static readonly CodeTest CodeTest = new CodeTest
        (
            new CodeTestConfiguration
            (
                ImmutableArray.Create
                (
                    MetadataReferenceUtility.MsCoreLib,
                    MetadataReferenceUtility.SystemRuntime,
                    MetadataReferenceUtility.NetStandard,
                    MetadataReferenceUtility.GetAssemblyReference<RpcContractAttribute>()
                )
            ).WithAdditionalGenerators(new Generator())
        )
        .WithAddedNamespaceImports("MsbRpc.Generator.Attributes")
        .InNamespace("MsbRpcTest.Serialization.ManualRpcTest.Incrementer.Input")
        .WithCode(Code);

    [TestMethod]
    public async Task GeneratorRuns()
    {
        CodeTestResult result = (await CodeTest.Run(CancellationToken, LoggerFactory)).Result;
        Assert.IsTrue(result.GeneratorResults.ContainsKey(typeof(Generator)));
    }

    [TestMethod]
    public async Task GeneratorHasSingleResult()
    {
        CodeTestResult result = await RunCodeTest();
        Assert.AreEqual(1, result.GeneratorResults.Count);
    }

    [TestMethod]
    public async Task GeneratorThrowsNoException()
    {
        GeneratorDriverRunResult rpcGeneratorResults = await RunGenerator();
        GeneratorRunResult rpcGeneratorResult = rpcGeneratorResults.Results[0];
        Assert.AreEqual(null, rpcGeneratorResult.Exception);
    }

    [TestMethod]
    public async Task GeneratorReportsNoDiagnostics()
    {
        GeneratorDriverRunResult rpcGeneratorResults = await RunGenerator();
        GeneratorRunResult rpcGeneratorResult = rpcGeneratorResults.Results[0];
        ImmutableArray<Diagnostic> diagnostics = rpcGeneratorResult.Diagnostics;
        Assert.AreEqual(0, diagnostics.Length);
    }

    [TestMethod]
    public async Task GeneratesOneOrMoreTrees() => Assert.That.HasGeneratedAnyTree(await RunGenerator());

    [TestMethod]
    public async Task GeneratesServerInterface()
    {
        const string expectedTreeName = "IIncrementerServer.g.cs";
        Assert.That.HasGeneratedAnyTree(await RunGenerator(), tree => tree.FilePath.EndsWith(expectedTreeName));
    }

    [TestMethod]
    public async Task GeneratesServerProcedureEnum()
    {
        const string expectedTreeName = "IncrementerServerProcedure.g.cs";
        Assert.That.HasGeneratedAnyTree(await RunGenerator(), tree => tree.FilePath.EndsWith(expectedTreeName));
    }

    [TestMethod]
    public async Task GeneratesServerProcedureEnumExtensions()
    {
        const string expectedTreeName = "IncrementerServerProcedureExtensions.g.cs";
        Assert.That.HasGeneratedAnyTree(await RunGenerator(), tree => tree.FilePath.EndsWith(expectedTreeName));
    }

    private async Task<CodeTestResult> RunCodeTest() => (await CodeTest.Run(CancellationToken, LoggerFactory)).Result;
    private async Task<GeneratorDriverRunResult> RunGenerator() => (await RunCodeTest()).GeneratorResults[typeof(Generator)].GetRunResult();
}
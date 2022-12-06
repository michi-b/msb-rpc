using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Misbat.CodeAnalysis.Test.CodeTest;
using Misbat.CodeAnalysis.Test.Utility;
using MsbRpc.Generator;
using MsbRpc.Generator.Attributes;

namespace MsbRpcTest.Serialization.ManualRpcTest.Incrementer.Tests;

[TestClass]
public class GeneratorTest : Test
{
    private const string Code = @"namespace MsbRpcTest.Serialization.ManualRpcTest.Incrementer.Input;

[RpcContract]
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
            ).WithAdditionalGenerators(new RpcGenerator())
        )
        .WithAddedNamespaceImports("MsbRpc.Generator.Attributes")
        .WithCode(Code);
    
    [TestMethod]
    public async Task GeneratorRuns()
    {
        CodeTestResult result = (await CodeTest.Run(CancellationToken)).Result;
        Assert.IsTrue(result.GeneratorResults.ContainsKey(typeof(RpcGenerator)));
    }

    [TestMethod]
    public async Task GeneratorHasSingleResult()
    {
        GeneratorDriverRunResult rpcGeneratorResults = await RunRpcGenerator();
        Assert.AreEqual(1, rpcGeneratorResults.Results.Length);
    }

    [TestMethod]
    public async Task GeneratorThrowsNoException()
    {
        GeneratorDriverRunResult rpcGeneratorResults = await RunRpcGenerator();
        GeneratorRunResult rpcGeneratorResult = rpcGeneratorResults.Results[0];
        Assert.AreEqual(null, rpcGeneratorResult.Exception);    
    }

    [TestMethod]
    public async Task GeneratorReportsNoDiagnostics()
    {
        GeneratorDriverRunResult rpcGeneratorResults = await RunRpcGenerator();
        GeneratorRunResult rpcGeneratorResult = rpcGeneratorResults.Results[0];
        ImmutableArray<Diagnostic> diagnostics = rpcGeneratorResult.Diagnostics;
        Assert.AreEqual(0, diagnostics.Length);
    }
    
    [TestMethod]
    public async Task GeneratesOneOrMoreTrees()
    {
        GeneratorDriverRunResult rpcGeneratorResults = await RunRpcGenerator();
        Assert.IsTrue(rpcGeneratorResults.GeneratedTrees.Length > 0, "generates one or more trees");
    }

    private async Task<GeneratorDriverRunResult> RunRpcGenerator()
    {
        CodeTestResult result = (await CodeTest.Run(CancellationToken)).Result;
        return result.GeneratorResults[typeof(RpcGenerator)].GetRunResult();
    }
}
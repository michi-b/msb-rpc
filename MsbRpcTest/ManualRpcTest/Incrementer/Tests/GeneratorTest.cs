﻿using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Misbat.CodeAnalysis.Test.CodeTest;
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

    [TestMethod]
    public async Task GeneratesServerInterface()
    {
        GeneratorDriverRunResult rpcGeneratorResults = await RunRpcGenerator();
        rpcGeneratorResults.GeneratedTrees.Any(tree => tree.FilePath.EndsWith("IIncrementerServer.cs"));
    }

    private async Task<GeneratorDriverRunResult> RunRpcGenerator()
    {
        CodeTestResult result = (await CodeTest.Run(CancellationToken, LoggerFactory)).Result;
        return result.GeneratorResults[typeof(Generator)].GetRunResult();
    }
}
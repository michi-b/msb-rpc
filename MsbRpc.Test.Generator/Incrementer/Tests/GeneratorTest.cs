﻿using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Misbat.CodeAnalysis.Test.CodeTest;
using Misbat.CodeAnalysis.Test.Extensions;
using Misbat.CodeAnalysis.Test.Utility;
using MsbRpc.EndPoints;
using MsbRpc.Generator;
using MsbRpc.Generator.Attributes;
using Serilog;
using Serilog.Core;

namespace MsbRpc.Test.Generator.Incrementer.Tests;

[TestClass]
public class GeneratorTest : Test
{
    private const string Code = @"[RpcContract]
public interface IIncrementer
{
    int Increment(int value);
    public void Store(int value);
    public void IncrementStored();
    public int GetStored();
}";

    private static readonly ILoggerFactory LoggerFactory;
    private static readonly ILogger<GeneratorTest> Logger;

    private static readonly CodeTest CodeTest = new CodeTest
        (
            new CodeTestConfiguration
            (
                ImmutableArray.Create
                (
                    MetadataReferenceUtility.MsCoreLib,
                    MetadataReferenceUtility.SystemRuntime,
                    MetadataReferenceUtility.NetStandard,
                    MetadataReferenceUtility.FromType<RpcContractAttribute>(),
                    MetadataReferenceUtility.FromType<EndPointDirection>(),
                    MetadataReferenceUtility.FromType<ILoggerFactory>(),
                    MetadataReferenceUtility.FromType<ArgumentOutOfRangeException>(),
                    MetadataReferenceUtility.TransitivelyReferenced(typeof(GeneratorTest), "System.Threading.Tasks.Extensions")
                )
            ).WithAdditionalGenerators(new ContractGenerator())
        )
        .WithAddedNamespaceImports("MsbRpc.Generator.Attributes")
        .InNamespace("MsbRpc.Test.Serialization.ManualRpcTest.Incrementer.Input")
        .WithCode(Code);

    static GeneratorTest()
    {
        Logger logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .CreateLogger()!;
        LoggerFactory = new LoggerFactory().AddSerilog(logger);
        Logger = LoggerFactory.CreateLogger<GeneratorTest>();
    }

    [TestMethod]
    public async Task GeneratorRuns()
    {
        CodeTestResult result = (await CodeTest.Run(CancellationToken, LoggerFactory)).Result;
        Assert.IsTrue(result.GeneratorResults.ContainsKey(typeof(ContractGenerator)));
    }

    [TestMethod]
    public async Task GeneratorHasOneResult()
    {
        CodeTestResult result = await RunCodeTest(loggingOptions: CodeTest.LoggingOptions.All);
        Assert.AreEqual(1, result.GeneratorResults.Count);
    }

    [TestMethod]
    public async Task GeneratorThrowsNoException()
    {
        GeneratorDriverRunResult rpcGeneratorResults = await RunGenerator(loggingOptions: CodeTest.LoggingOptions.None);
        GeneratorRunResult rpcGeneratorResult = rpcGeneratorResults.Results[0];
        Assert.AreEqual(null, rpcGeneratorResult.Exception);
    }

    [TestMethod]
    public async Task GeneratorReportsNoDiagnostics()
    {
        GeneratorDriverRunResult rpcGeneratorResults = await RunGenerator(loggingOptions: CodeTest.LoggingOptions.GeneratorDiagnostics);
        GeneratorRunResult rpcGeneratorResult = rpcGeneratorResults.Results[0];
        ImmutableArray<Diagnostic> diagnostics = rpcGeneratorResult.Diagnostics;
        Assert.AreEqual(0, diagnostics.Length);
    }

    [TestMethod]
    public async Task FinalCompilationReportsNoDiagnostics()
    {
        CodeTestResult result = await RunCodeTest(loggingOptions: CodeTest.LoggingOptions.FinalDiagnostics);
        ImmutableArray<Diagnostic> diagnostics = result.Compilation.GetDiagnostics();
        Assert.AreEqual(0, diagnostics.Length);
    }

    [TestMethod]
    public async Task GeneratesOneOrMoreTrees() => Assert.That.HasGeneratedAnyTree(await RunGenerator(loggingOptions: CodeTest.LoggingOptions.Code));

    [TestMethod]
    public async Task GeneratesServerProcedureEnum()
    {
        await TestGenerates("IncrementerServerProcedure.g.cs");
    }

    // [TestMethod]
    // public async Task GeneratesServerProcedureEnumExtensions()
    // {
    //     await TestGenerates("IncrementerServerProcedureExtensions.g.cs");
    // }

    // [TestMethod]
    // public async Task GeneratesClientEndPoint()
    // {
    //     await TestGenerates("IncrementerClientEndPoint.g.cs");
    // }

    // [TestMethod]
    // public async Task GeneratesServerEndPoint()
    // {
    //     await TestGenerates("IncrementerServerEndPoint.g.cs");
    // }

    //todo: test generation of server (server endpoint factory)
    
    private async Task TestGenerates(string shortFileName)
    {
        bool IsTargetDiagnostic
            (Diagnostic diagnostic)
            => diagnostic.Location.SourceTree == null || diagnostic.Location.SourceTree.GetShortFilename() == shortFileName;

        GeneratorDriverRunResult result = await RunGenerator(IsTargetDiagnostic);
        SyntaxTree? tree = result.GeneratedTrees.FirstOrDefault(tree => tree.GetShortFilename() == shortFileName);
        Assert.IsNotNull(tree);
        await Logger.LogTreeAsync(tree, nameof(GeneratesServerProcedureEnum), CancellationToken);
        Logger.LogInformation("Full file path is '{TreeFilePath}'", tree.FilePath);
    }

    private async Task<GeneratorDriverRunResult> RunGenerator
    (
        Predicate<Diagnostic>? diagnosticFilter = null,
        CodeTest.LoggingOptions loggingOptions = CodeTest.LoggingOptions.Diagnostics
    )
        => (await RunCodeTest(diagnosticFilter, loggingOptions)).GeneratorResults[typeof(ContractGenerator)].GetRunResult();

    private async Task<CodeTestResult> RunCodeTest
    (
        Predicate<Diagnostic>? diagnosticFilter = null,
        CodeTest.LoggingOptions loggingOptions = CodeTest.LoggingOptions.None
    )
    {
        CodeTest codeTest = diagnosticFilter != null
            ? CodeTest.Configure(configuration => configuration.WithAdditionalDiagnosticFilters(diagnosticFilter))
            : CodeTest;
        return (await codeTest.Run(CancellationToken, LoggerFactory, loggingOptions)).Result;
    }
}
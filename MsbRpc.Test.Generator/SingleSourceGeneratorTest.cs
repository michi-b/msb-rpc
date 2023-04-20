using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Misbat.CodeAnalysis.Test.CodeTest;
using Misbat.CodeAnalysis.Test.Extensions;
using MsbRpc.Contracts;
using MsbRpc.Generator;
using MsbRpc.Generator.Attributes;
using MsbRpc.Test.Generator.Incrementer.Tests;
using Serilog;
using Serilog.Core;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace MsbRpc.Test.Generator;

public abstract class SingleSourceGeneratorTest<TTest, TGenerator> : Misbat.CodeAnalysis.Test.TestBases.SingleSourceGeneratorTest<TTest, TGenerator>
    where TTest : SingleSourceGeneratorTest<TTest, TGenerator>
    where TGenerator : IIncrementalGenerator, new()
{
    private readonly ILoggerFactory _loggerFactory;

    private readonly ILogger<TTest> _typedLogger;
    
    protected override ILogger Logger => _typedLogger;

    // ReSharper disable once StaticMemberInGenericType
    // yeah let's ignore this
    private static readonly Type[] StaticReferencedTypes = { typeof(RpcContractAttribute), typeof(IRpcContract) }; 
    
    protected override Type[] ReferencedTypes => StaticReferencedTypes;

    protected SingleSourceGeneratorTest()
    {
        Logger logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .CreateLogger()!;
        _loggerFactory = new LoggerFactory().AddSerilog(logger);
        _typedLogger = _loggerFactory.CreateLogger<TTest>();
    }

    [PublicAPI]
    protected async Task<CodeTestResult> RunCodeTest
    (
        CodeTest.LoggingOptions loggingOptions = CodeTest.LoggingOptions.None,
        Predicate<Diagnostic>? diagnosticFilter = null
    )
        => (await CodeTest.Run(CancellationToken, _loggerFactory, loggingOptions)).Result;

    [PublicAPI]
    protected async Task LogDiagnosticSourceTrees(CodeTestResult result, ImmutableArray<Diagnostic> diagnostics)
    {
        IEnumerable<string> diagnosticTargetFilePaths = from diagnostic in diagnostics
            where diagnostic.Location.SourceTree != null
            select diagnostic.Location.SourceTree.FilePath;
        foreach (string filePath in diagnosticTargetFilePaths.Distinct())
        {
            // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
            foreach (KeyValuePair<Type, GeneratorDriver> generatorResult in result.GeneratorResults)
            {
                ImmutableArray<SyntaxTree> syntaxTrees = generatorResult.Value.GetRunResult().GeneratedTrees;
                foreach (SyntaxTree tree in syntaxTrees.Where(tree => tree.FilePath == filePath))
                {
                    await _typedLogger.LogTreeAsync(tree, filePath, CancellationToken);
                }
            }
        }
    }

    protected async Task TestGeneratorRuns()
    {
        CodeTestResult result = await RunCodeTest();
        Assert.IsTrue(result.GeneratorResults.ContainsKey(typeof(TGenerator)));
    }

    protected async Task TestGeneratesAnyTrees()
        => Assert.That.HasGeneratedAnyTree((await RunCodeTest(CodeTest.LoggingOptions.All)).GeneratorResults[typeof(ContractGenerator)].GetRunResult());

    protected async Task TestFinalCompilationReportsNoDiagnostics()
    {
        CodeTestResult result = await RunCodeTest(CodeTest.LoggingOptions.FinalDiagnostics);
        ImmutableArray<Diagnostic> diagnostics = result.Compilation.GetDiagnostics();

        await LogDiagnosticSourceTrees(result, diagnostics);

        Assert.AreEqual(0, diagnostics.Length);
    }

    protected async Task TestGeneratesFile(string shortFileName)
    {
        bool IsTargetDiagnostic(Diagnostic diagnostic) => diagnostic.Location.SourceTree == null || diagnostic.Location.SourceTree.GetShortFilename() == shortFileName;

        Predicate<Diagnostic> diagnosticFilter = IsTargetDiagnostic;
        GeneratorDriverRunResult result = GetGeneratorDriverRunResult(await RunCodeTest(CodeTest.LoggingOptions.Diagnostics, diagnosticFilter));
        SyntaxTree? tree = result.GeneratedTrees.FirstOrDefault(tree => tree.GetShortFilename() == shortFileName);
        Assert.IsNotNull(tree);
        await _typedLogger.LogTreeAsync(tree, nameof(GeneratorTest.GeneratesServerProcedureEnum), CancellationToken);
        _typedLogger.LogInformation("Full file path is '{TreeFilePath}'", tree.FilePath);
    }

    protected async Task TestGeneratorHasOneResult() => Assert.AreEqual(1, (await RunCodeTest(CodeTest.LoggingOptions.All)).GeneratorResults.Count);

    protected async Task TestGeneratorThrowsNoException() => Assert.AreEqual(null, GetGeneratorRunResult(await RunCodeTest()).Exception);

    protected async Task TestGeneratorReportsNoDiagnostics()
    {
        ImmutableArray<Diagnostic> diagnostics = GetGeneratorRunResult(await RunCodeTest(CodeTest.LoggingOptions.GeneratorDiagnostics)).Diagnostics;
        Assert.AreEqual(0, diagnostics.Length);
    }

    [PublicAPI]
    protected static GeneratorRunResult GetGeneratorRunResult(CodeTestResult result) => GetGeneratorRunResult(GetGeneratorDriverRunResult(result));

    [PublicAPI]
    protected static GeneratorRunResult GetGeneratorRunResult(GeneratorDriverRunResult generatorResults) => generatorResults.Results[0];

    [PublicAPI]
    protected static GeneratorDriverRunResult GetGeneratorDriverRunResult(CodeTestResult result) => result.GeneratorResults[typeof(ContractGenerator)].GetRunResult();
}
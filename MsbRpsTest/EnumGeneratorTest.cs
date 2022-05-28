using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Misbat.CodeAnalysis.Test.CodeTest;
using MsbRps.CodeAnalysis;
using MsbRpsTest.Utility;

namespace MsbRpsTest;

[TestClass]
public class EnumGeneratorTest
{
    private static readonly CodeTest CodeTest = CodeTestUtility.Default
        .Configure(configuration => configuration.WithAdditionalGenerators(new EnumGenerator()));

    // ReSharper disable once MemberCanBePrivate.Global
    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
    // Injected by MSTest
    public TestContext TestContext { private get; set; } = null!;

    private CancellationToken CancellationToken => TestContext.CancellationTokenSource.Token;

    [TestMethod]
    public void TestsRun() { }

    [TestMethod]
    public async Task GeneratesStaticCode()
    {
        CodeTestResult result = (await CodeTest.WithCode("").Run(CancellationToken)).Result;
        GeneratorDriverRunResult enumGeneratorResult = result.GeneratorResults[typeof(EnumGenerator)].GetRunResult();
        Assert.AreEqual(1, enumGeneratorResult.GeneratedTrees.Length);
    }

    [TestMethod]
    public async Task EnumExtensionsGeneratesCode()
    {
        const string code = @"
[EnumExtensions]
public enum TestEnum{}";

        CodeTestResult result = (await CodeTest
            .WithAddedNamespaceImports("NetEscapades.EnumGenerators")
            .WithCode(code)
            .Run(CancellationToken)).Result;
        GeneratorDriverRunResult enumGeneratorResult = result.GeneratorResults[typeof(EnumGenerator)].GetRunResult();
        Assert.AreEqual(2, enumGeneratorResult.GeneratedTrees.Length);
    }
}
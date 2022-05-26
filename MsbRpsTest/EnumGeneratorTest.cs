using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Misbat.CodeAnalysis.Test.CodeTest;
using MsbRps.CodeAnalysis;
using MsbRpsTest.Utility;

namespace MsbRpsTest;

[TestClass]
public class EnumGeneratorTest
{
    private static readonly CodeTest CodeTest = CodeTestUtility.Default.WithGenerator(new EnumGenerator());

    public TestContext TestContext { get; set; } = null!;

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
}
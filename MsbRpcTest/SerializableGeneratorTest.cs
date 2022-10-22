#if false
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Misbat.CodeAnalysis.Test.CodeTest;
using Misbat.CodeAnalysis.Test.Utility;
using MsbRpc.Generator;
using MsbRpc.GeneratorAttributes;

namespace MsbRpcTest;

[TestClass]
public class SerializableGeneratorTest : Test
{
    private static readonly CodeTest CodeTest = new CodeTest
        (
            new CodeTestConfiguration
            (
                ImmutableArray.Create
                (
                    MetadataReferenceUtility.MsCoreLib,
                    MetadataReferenceUtility.SystemRuntime,
                    MetadataReferenceUtility.NetStandard,
                    MetadataReferenceUtility.GetAssemblyReference<RpcInterfaceAttribute>()
                )
            ).WithAdditionalGenerators(new RpcGenerator())
        )
        .WithAddedNamespaceImports("MsbRps.GeneratorAttributes");

    [TestMethod]
    public async Task ValidClassGeneratesSerialization()
    {
        const string code = @"
[MsbRpsObject]
public partial struct Test {}";

        CodeTestResult result = (await CodeTest.WithCode(code).Run(CancellationToken)).Result;
        GeneratorDriverRunResult serializationGeneratorResult = result.GeneratorResults[typeof(RpcGenerator)].GetRunResult();
        Assert.AreEqual(1, serializationGeneratorResult.GeneratedTrees.Length);
    }
}
#endif
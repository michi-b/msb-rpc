using Misbat.CodeAnalysis.Test.CodeTest;

namespace MsbRpc.Test.Generator;

public static class CodeTestUtility
{
    public static CodeTest GetContractGeneratorCodeTest<TTest>(string code, string nameSpace) where TTest : Test
        => new CodeTest(CodeTestConfigurationUtility.GetContractGeneratorCodeTestConfiguration<TTest>())
            .WithAddedNamespaceImports("MsbRpc.Generator.Attributes", "MsbRpc.Contracts")
            .InNamespace(nameSpace)
            .WithCode(code);
}
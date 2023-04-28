using Misbat.CodeAnalysis.Test.CodeTest;

namespace MsbRpc.Test.Generator;

public static class CodeTestUtility
{
    public static CodeTest Configure(CodeTest codeTest)
        => codeTest.WithAddedNamespaceImports
        (
            "System",
            "MsbRpc.Generator.Attributes",
            "MsbRpc.Contracts",
            "MsbRpc.Serialization.Buffers",
            "MsbRpc.Serialization.Primitives",
            "MsbRpc.Generator.Attributes.Serialization"
        );
}
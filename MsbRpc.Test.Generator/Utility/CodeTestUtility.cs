#region

using Misbat.CodeAnalysis.Test.CodeTest;

#endregion

namespace MsbRpc.Test.Generator.Utility;

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
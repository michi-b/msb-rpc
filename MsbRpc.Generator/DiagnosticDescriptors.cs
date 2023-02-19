using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;

namespace MsbRpc.Generator;

// diagnostic descriptor analyzer has trouble with target-typed new
[SuppressMessage("ReSharper", "ArrangeObjectCreationWhenTypeEvident")]
public static class DiagnosticDescriptors
{
    public static readonly DiagnosticDescriptor ContractGeneratorException = new DiagnosticDescriptor
    (
        "MR1000",
        "Contract Generator Exception",
        "generator for contract '{0}' is skipped due to an exception ({1}) while generating code: '{2}'",
        "Generator",
        DiagnosticSeverity.Error,
        true
    );

    public static readonly DiagnosticDescriptor TypeIsNotAValidRpcParameter = new DiagnosticDescriptor
    (
        "MR1001",
        "Invalid RPC Parameter Type",
        "parameter '{2}' at position {3} has type '{4}' with serialization kind '{5}', which is not a valid RPC parameter type"
        + ", and will therefore be replaced with the default value in calls to {0}.{1}",
        "Generator",
        DiagnosticSeverity.Warning,
        true
    );

    public static readonly DiagnosticDescriptor TypeIsNotAValidRpcReturnType = new DiagnosticDescriptor
    (
        "MR1002",
        "Invalid RPC Return Type",
        "RPC return type '{2}' with serialization kind '{3}' is not a valid RPC return type"
        + ", and will therefore not be transmitted back from {0}.{1}",
        "Generator",
        DiagnosticSeverity.Warning,
        true
    );
}
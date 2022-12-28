using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;

namespace MsbRpc.Generator;

// diagnostic descriptor analyzer has trouble with target-typed new
[SuppressMessage("ReSharper", "ArrangeObjectCreationWhenTypeEvident")]
public static class DiagnosticDescriptors
{
    public static readonly DiagnosticDescriptor ContractGeneratorException = new DiagnosticDescriptor
    (
        "MR0999",
        "Contract Generator Exception",
        "generator for contract '{0}' is skipped due to an exception ({1}) while generating code: '{2}'",
        "Generator",
        DiagnosticSeverity.Error,
        true
    );

    public static readonly DiagnosticDescriptor ContractGeneratorError = new DiagnosticDescriptor
    (
        "MR1000",
        "Contract Generator Error",
        "contract generator skipped code generation for contract '{0}' due to previous errors",
        "Generator",
        DiagnosticSeverity.Error,
        true
    );

    public static readonly DiagnosticDescriptor TypeIsNotAValidRpcParameter = new DiagnosticDescriptor
    (
        "MR1001",
        "Invalid RPC Parameter Type",
        "the type {0} with serialization kind {1} is not a valid RPC parameter type",
        "Generator",
        DiagnosticSeverity.Error,
        true
    );

    public static readonly DiagnosticDescriptor TypeIsNotAValidRpcReturnType = new DiagnosticDescriptor
    (
        "MR1002",
        "Invalid RPC Return Type",
        "The type {0} with serialization kind {1} is not a valid return type for an RPC method",
        "Generator",
        DiagnosticSeverity.Error,
        true
    );
}
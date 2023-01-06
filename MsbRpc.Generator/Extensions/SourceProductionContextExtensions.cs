using System;
using Microsoft.CodeAnalysis;
using MsbRpc.Generator.CodeWriters.Files;
using MsbRpc.Generator.GenerationTree;
using MsbRpc.Generator.Info;
using static MsbRpc.Generator.DiagnosticDescriptors;

namespace MsbRpc.Generator.Extensions;

internal static class SourceProductionContextExtensions
{
    public static void ReportTypeIsNotAValidRpcParameter(this SourceProductionContext context, TypeNode typeNode)
    {
        context.ReportDiagnostic
        (
            Diagnostic.Create
            (
                TypeIsNotAValidRpcParameter,
                Location.None,
                typeNode.FullName,
                typeNode.SerializationKind.GetName()
            )
        );
    }

    public static void ReportTypeIsNotAValidRpcReturnType(this SourceProductionContext context, TypeNode typeNode)
    {
        context.ReportDiagnostic
        (
            Diagnostic.Create
            (
                TypeIsNotAValidRpcReturnType,
                Location.None,
                typeNode.FullName,
                typeNode.SerializationKind.GetName()
            )
        );
    }

    public static void ReportInvalidContract(this SourceProductionContext context, ContractNode contract)
    {
        context.ReportDiagnostic(Diagnostic.Create(InvalidGeneratorContract, Location.None, contract.PascalCaseName));
    }

    public static void ReportContractGenerationException(this SourceProductionContext context, ref ContractInfo contractInfo, Exception exception)
    {
        context.ReportDiagnostic
        (
            Diagnostic.Create
            (
                ContractGeneratorException,
                Location.None,
                $"{contractInfo.Namespace}.{contractInfo.InterfaceName}",
                exception.GetType().ToString(),
                exception.Message
            )
        );
    }

    public static void GenerateFile(this SourceProductionContext context, CodeFileWriter writer)
    {
        CodeFileWriter.Result result = writer.Generate();
        context.AddSource(result.FileName, result.Code);
    }
}
using System;
using Microsoft.CodeAnalysis;
using MsbRpc.Generator.CodeWriters.Files.Base;
using MsbRpc.Generator.GenerationTree;
using MsbRpc.Generator.Info;
using static MsbRpc.Generator.DiagnosticDescriptors;

namespace MsbRpc.Generator.Extensions;

internal static class SourceProductionContextExtensions
{
    public static void ReportTypeIsNotAValidRpcParameter
    (
        this SourceProductionContext context,
        ContractNode contract,
        ProcedureNode procedure,
        ParameterNode parameter
    )
    {
        context.ReportDiagnostic
        (
            Diagnostic.Create
            (
                TypeIsNotAValidRpcParameter,
                Location.None,
                contract.InterfaceName,
                procedure.Name,
                parameter.Name,
                parameter.Index.ToString(),
                parameter.Serialization.DeclarationSyntax
            )
        );
    }

    public static void ReportTypeIsNotAValidRpcReturnType
    (
        this SourceProductionContext context,
        ContractNode contract,
        ProcedureNode procedure
    )
    {
        context.ReportDiagnostic
        (
            Diagnostic.Create
            (
                TypeIsNotAValidRpcReturnType,
                Location.None,
                contract.InterfaceName,
                procedure.Name,
                procedure.ResultSerialization.DeclarationSyntax
            )
        );
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
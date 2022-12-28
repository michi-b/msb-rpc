﻿using System;
using Microsoft.CodeAnalysis;
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
                typeNode.Names.FullName,
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
                typeNode.Names.FullName,
                typeNode.SerializationKind.GetName()
            )
        );
    }

    public static void ReportContractGenerationError(this SourceProductionContext context, ContractNode contract)
    {
        context.ReportDiagnostic(Diagnostic.Create(ContractGeneratorError, Location.None, contract.Names.PascalCaseName));
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
}
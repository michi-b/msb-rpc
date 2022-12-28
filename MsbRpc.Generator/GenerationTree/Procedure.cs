using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using MsbRpc.Generator.Extensions;
using MsbRpc.Generator.GenerationTree.Names;
using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.GenerationTree;

internal class Procedure
{
    public readonly string EnumValueString;
    public readonly bool InvertsDirection;
    public readonly bool IsValid;
    public readonly ProcedureNames Names;
    public readonly ParameterCollection? Parameters;
    public readonly TypeNode ReturnType;

    public Procedure
    (
        ProcedureInfo procedureInfo,
        ProcedureCollectionNames procedureCollectionNames,
        int definitionIndex,
        TypeNodeCache typeNodeCache,
        SourceProductionContext context
    )
    {
        Names = new ProcedureNames(procedureCollectionNames, procedureInfo);

        ReturnType = typeNodeCache.GetOrAdd(procedureInfo.ReturnType, context);
        if (!ReturnType.IsValidReturnType)
        {
            context.ReportTypeIsNotAValidRpcReturnType(ReturnType);
        }

        IsValid = ReturnType.IsValidReturnType;

        EnumValueString = definitionIndex.ToString();
        InvertsDirection = procedureInfo.InvertsDirection;

        ImmutableArray<ParameterInfo> parameterInfos = procedureInfo.Parameters;
        if (parameterInfos.Length > 0)
        {
            Parameters = new ParameterCollection(parameterInfos, typeNodeCache, context);
            IsValid = IsValid && Parameters.IsValid;
        }
        else
        {
            Parameters = null;
        }
    }
}
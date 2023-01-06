using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using MsbRpc.Generator.Extensions;
using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.GenerationTree;

internal class ProcedureNode
{
    public readonly ProcedureCollectionNode CollectionNode;
    public readonly bool HasParameters;
    public readonly bool HasReturnValue;
    public readonly bool IsValid;

    public readonly string Name;
    public readonly ParameterCollectionNode? Parameters;
    public readonly string ProcedureEnumIntValue;

    public readonly string ProcedureEnumValue;
    public readonly TypeNode ReturnType;

    public ProcedureNode
    (
        ProcedureInfo info,
        ProcedureCollectionNode collectionNode,
        int definitionIndex,
        TypeNodeCache typeNodeCache,
        SourceProductionContext context
    )
    {
        CollectionNode = collectionNode;

        Name = info.Name;
        ProcedureEnumValue = $"{collectionNode.ProcedureEnumName}.{Name}";
        ProcedureEnumIntValue = definitionIndex.ToString();

        ReturnType = typeNodeCache.GetOrAdd(info.ReturnType);
        if (!ReturnType.IsValidReturnType)
        {
            context.ReportTypeIsNotAValidRpcReturnType(ReturnType);
        }

        HasReturnValue = !ReturnType.IsVoid;

        IsValid = ReturnType.IsValidReturnType;

        ImmutableArray<ParameterInfo> parameterInfos = info.Parameters;
        HasParameters = parameterInfos.Length > 0;
        if (HasParameters)
        {
            Parameters = new ParameterCollectionNode(parameterInfos, typeNodeCache, context);
            IsValid = IsValid && Parameters.IsValid;
        }
        else
        {
            Parameters = null;
        }
    }
}
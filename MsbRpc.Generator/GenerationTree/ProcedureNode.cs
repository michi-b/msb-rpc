using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using MsbRpc.Generator.Extensions;
using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.GenerationTree;

internal class ProcedureNode
{
    public readonly string AsyncName;
    public readonly ProcedureCollectionNode CollectionNode;
    public readonly string EnumValueString;
    public readonly int IntValue;
    public readonly string IntValueString;
    public readonly bool IsValid;

    public readonly string Name;
    public readonly ParameterCollectionNode? Parameters;
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
        AsyncName = $"{Name}{IndependentNames.AsyncPostFix}";
        EnumValueString = $"{collectionNode.EnumName}.{Name}";
        IntValue = definitionIndex;
        IntValueString = definitionIndex.ToString();

        ReturnType = typeNodeCache.GetOrAdd(info.ReturnType);
        if (!ReturnType.IsValidReturnType)
        {
            context.ReportTypeIsNotAValidRpcReturnType(ReturnType);
        }

        IsValid = ReturnType.IsValidReturnType;

        ImmutableArray<ParameterInfo> parameterInfos = info.Parameters;
        if (parameterInfos.Length > 0)
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
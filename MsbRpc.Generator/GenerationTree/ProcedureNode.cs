using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using MsbRpc.Generator.Extensions;
using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.GenerationTree;

internal class ProcedureNode
{
    public readonly string AsyncName;
    public readonly ProcedureCollection Collection;
    public readonly string EnumValueString;
    public readonly int IntValue;
    public readonly string IntValueString;
    public readonly bool IsValid;

    public readonly string Name;
    public readonly ParameterCollection? Parameters;
    public readonly TypeNode ReturnType;

    public ProcedureNode
    (
        ProcedureInfo info,
        ProcedureCollection collection,
        int definitionIndex,
        TypeNodeCache typeNodeCache,
        SourceProductionContext context
    )
    {
        Collection = collection;

        Name = info.Name;
        AsyncName = $"{Name}{IndependentNames.AsyncPostFix}";
        EnumValueString = $"{collection.EnumName}.{Name}";
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
            Parameters = new ParameterCollection(parameterInfos, typeNodeCache, context);
            IsValid = IsValid && Parameters.IsValid;
        }
        else
        {
            Parameters = null;
        }
    }
}
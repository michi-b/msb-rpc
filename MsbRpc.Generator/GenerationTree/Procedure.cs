using System.Collections.Immutable;
using MsbRpc.Generator.GenerationTree.Names;
using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.GenerationTree;

public class Procedure
{
    public readonly string EnumValueString;
    public readonly bool InvertsDirection;
    public readonly ProcedureNames Names;
    public readonly ParameterCollection? Parameters;
    public readonly TypeNode ReturnType;

    public Procedure(ProcedureInfo procedureInfo, ProcedureCollectionNames procedureCollectionNames, int definitionIndex, TypeCache typeCache)
    {
        Names = new ProcedureNames(procedureCollectionNames, procedureInfo);
        ReturnType = typeCache.GetOrAdd(procedureInfo.ReturnType);
        EnumValueString = definitionIndex.ToString();
        InvertsDirection = procedureInfo.InvertsDirection;

        ImmutableArray<ParameterInfo> parameterInfos = procedureInfo.Parameters;
        Parameters = parameterInfos.Length > 0
            ? new ParameterCollection(parameterInfos, typeCache)
            : null;
    }
}
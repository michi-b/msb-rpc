using System.Collections.Immutable;
using MsbRpc.Generator.HelperTree.Names;
using MsbRpc.Generator.Info;
using static MsbRpc.Generator.IndependentNames;

namespace MsbRpc.Generator.HelperTree;

public class Procedure
{
    private readonly ParameterCollection? _parameters;
    public readonly int EnumValue;
    public readonly string EnumValueString;
    public readonly bool HasParameters;
    public readonly bool InvertsDirection;
    public readonly ProcedureNames Names;
    public readonly string ReadResultLine;
    public readonly TypeNode ReturnType;

    public Procedure(ProcedureInfo procedureInfo, ProcedureCollectionNames procedureCollectionNames, int definitionIndex, TypeCache typeCache)
    {
        Names = new ProcedureNames(procedureCollectionNames, procedureInfo);
        ReturnType = typeCache.GetOrAdd(procedureInfo.ReturnType);
        EnumValue = definitionIndex;
        EnumValueString = definitionIndex.ToString();
        InvertsDirection = procedureInfo.InvertsDirection;

        ImmutableArray<ParameterInfo> parameterInfos = procedureInfo.Parameters;
        HasParameters = parameterInfos.Length > 0;
        if (HasParameters)
        {
            _parameters = new ParameterCollection(parameterInfos, typeCache);
            HasParameters = true;
        }

        ReadResultLine =
            $"{ReturnType.Names.Name} {Variables.Result} = {Variables.ResultReader}.{ReturnType.SerializationKind.GetBufferReadMethodName()}();";
    }

    public bool TryGetParameters(out ParameterCollection? parameters)
    {
        if (HasParameters)
        {
            parameters = _parameters;
            return true;
        }

        parameters = null;
        return false;
    }
}
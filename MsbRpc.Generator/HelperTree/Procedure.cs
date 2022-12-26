using System.Collections.Immutable;
using MsbRpc.Generator.HelperTree.Names;
using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.HelperTree;

public class Procedure
{
    public readonly TypeNode ReturnType;
    public readonly ProcedureNames Names;
    public readonly int EnumValue;
    public readonly string EnumValueString;
    public readonly bool InvertsDirection;
    public readonly int LastParameterIndex;
    public readonly int ParameterCount;
    public readonly bool HasParameters;
    
    private readonly Parameter[]? Parameters;

    public Procedure(ProcedureInfo procedureInfo, ProcedureCollectionNames procedureCollectionNames, int definitionIndex, TypeCache typeCache)
    {
        Names = new ProcedureNames(procedureCollectionNames, procedureInfo);
        ReturnType = typeCache.GetOrAdd(procedureInfo.ReturnType);
        EnumValue = definitionIndex;
        EnumValueString = definitionIndex.ToString();
        InvertsDirection = procedureInfo.InvertsDirection;
        
        ImmutableArray<ParameterInfo> parameterInfos = procedureInfo.Parameters;
        ParameterCount = parameterInfos.Length;
        HasParameters = ParameterCount > 0;
        if (HasParameters)
        {
            int parameterCount = ParameterCount;
            LastParameterIndex = parameterCount - 1;
            Parameters = new Parameter[parameterCount];
            for (int i = 0; i < ParameterCount; i++)
            {
                ParameterInfo parameterInfo = parameterInfos[i];
                Parameters[i] = new Parameter(parameterInfo.Name, typeCache.GetOrAdd(parameterInfo.Type));
            }
        }
        else
        {
            LastParameterIndex = -1;
            Parameters = null;
        }
    }

    public bool TryGetParameters(out Parameter[]? parameters)
    {
        if (HasParameters)
        {
            parameters = Parameters;
            return true;
        }

        parameters = null;
        return false;
    }
}
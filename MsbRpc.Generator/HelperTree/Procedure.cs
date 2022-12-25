using System.Collections.Immutable;
using MsbRpc.Generator.HelperTree.Names;
using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.HelperTree;

public class Procedure
{
    public readonly TypeNode ReturnType;
    public readonly Parameter[] Parameters;
    public readonly ProcedureNames Names;
    // ReSharper disable once MemberCanBePrivate.Global
    public readonly int EnumValue;
    public readonly string EnumValueString;
    public readonly bool InvertsDirection;
    public readonly int LastParameterIndex;
    
    public Procedure(ProcedureInfo procedureInfo, ProcedureCollectionNames procedureCollectionNames, int definitionIndex, TypeCache typeCache)
    {
        Names = new ProcedureNames(procedureCollectionNames, procedureInfo);
        ReturnType = typeCache.GetOrAdd(procedureInfo.ReturnType);
        EnumValue = definitionIndex;
        EnumValueString = definitionIndex.ToString();
        InvertsDirection = procedureInfo.InvertsDirection;
        
        ImmutableArray<ParameterInfo> parameterInfos = procedureInfo.Parameters;
        int parameterCount = parameterInfos.Length;
        LastParameterIndex = parameterCount - 1;
        Parameters = new Parameter[parameterCount];
        for (int i = 0; i < parameterInfos.Length; i++)
        {
            ParameterInfo parameterInfo = parameterInfos[i];
            Parameters[i] = new Parameter(parameterInfo.Name, typeCache.GetOrAdd(parameterInfo.Type));
        }
    }
}
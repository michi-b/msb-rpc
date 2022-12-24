using System.Collections.Immutable;
using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.HelperTree;

public class Procedure
{
    public readonly TypeNode ReturnType;
    public readonly Parameter[] Parameters;
    public readonly ProcedureNames Names;
    public readonly int EnumValue;
    public readonly string EnumValueString;
    
    public Procedure(ProcedureInfo procedureInfo, ProcedureCollectionNames procedureCollectionNames, int definitionIndex, TypeCache typeCache)
    {
        Names = new ProcedureNames(procedureCollectionNames, procedureInfo);
        ReturnType = typeCache.GetOrAdd(procedureInfo.ReturnType);
        EnumValue = definitionIndex;
        EnumValueString = definitionIndex.ToString();

        ImmutableArray<ParameterInfo> parameterInfos = procedureInfo.Parameters;
        int parameterCount = parameterInfos.Length;
        Parameters = new Parameter[parameterCount];
        for (int i = 0; i < parameterInfos.Length; i++)
        {
            ParameterInfo parameterInfo = parameterInfos[i];
            Parameters[i] = new Parameter(parameterInfo.Name, typeCache.GetOrAdd(parameterInfo.Type));
        }
    }
}

public readonly struct Parameter
{
    public Parameter(string name, TypeNode type)
    {
        Name = name;
        Type = type;
    }

    public string Name { get; }
    public TypeNode Type { get; }
}
using System.Collections.Immutable;
using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.GeneratorHelperTree;

public class ProcedureNode
{
    public readonly TypeNode ReturnType;
    public readonly string Name;
    public readonly Parameter[] Parameters;
    
    public ProcedureNode(ContractNode contractNode, ref ProcedureInfo procedureInfo, TypeCache typeCache)
    {
        Name = procedureInfo.Name;

        ReturnType = typeCache.GetOrAdd(procedureInfo.ReturnType);

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
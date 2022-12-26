using System.Collections.Immutable;
using MsbRpc.Generator.HelperTree.Names;
using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.HelperTree;

public class Procedure
{
    private readonly bool _hasParameters;

    private readonly ParameterCollection? _parameters;
    public readonly string EnumValueString;
    public readonly bool InvertsDirection;
    public readonly ProcedureNames Names;
    public readonly TypeNode ReturnType;

    public Procedure(ProcedureInfo procedureInfo, ProcedureCollectionNames procedureCollectionNames, int definitionIndex, TypeCache typeCache)
    {
        Names = new ProcedureNames(procedureCollectionNames, procedureInfo);
        ReturnType = typeCache.GetOrAdd(procedureInfo.ReturnType);
        EnumValueString = definitionIndex.ToString();
        InvertsDirection = procedureInfo.InvertsDirection;

        ImmutableArray<ParameterInfo> parameterInfos = procedureInfo.Parameters;
        _hasParameters = parameterInfos.Length > 0;
        if (_hasParameters)
        {
            _parameters = new ParameterCollection(parameterInfos, typeCache);
            _hasParameters = true;
        }
    }

    public bool TryGetParameters(out ParameterCollection? parameters)
    {
        if (_hasParameters)
        {
            parameters = _parameters;
            return true;
        }

        parameters = null;
        return false;
    }
}
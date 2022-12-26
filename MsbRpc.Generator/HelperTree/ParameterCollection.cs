using System.Collections;
using System.Collections.Immutable;
using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.HelperTree;

public class ParameterCollection : IReadOnlyList<Parameter>
{
    private readonly Parameter[] _parameters;
    public readonly int LastIndex;
    public readonly IReadOnlyList<Parameter> ConstantSizeParameters;
    public readonly bool HasOnlyConstantSizeParameters = true;
    
    public ParameterCollection(ImmutableArray<ParameterInfo> parameterInfos, TypeCache typeCache)
    {
        Count = parameterInfos.Length;
        LastIndex = Count - 1;
        _parameters = new Parameter[Count];
        List<Parameter> constantSizeParameters = new(Count);
        for (int i = 0; i < Count; i++)
        {
            ParameterInfo parameterInfo = parameterInfos[i];
            var parameter = new Parameter(parameterInfo.Name, typeCache.GetOrAdd(parameterInfo.Type));
            _parameters[i] = parameter;
            
            if(parameter.Type.IsConstantSize)
            {
                constantSizeParameters.Add(parameter);
            }
            else
            {
                HasOnlyConstantSizeParameters = false;
            }
        }
        ConstantSizeParameters = constantSizeParameters;
    }

    public IEnumerator<Parameter> GetEnumerator() => ((IEnumerable<Parameter>)_parameters).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    
    public int Count { get; }

    public Parameter this[int index] => _parameters[index];
}
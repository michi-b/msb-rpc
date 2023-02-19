using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.GenerationTree;

internal class ParameterCollectionNode : IReadOnlyList<ParameterNode>
{
    private readonly ParameterNode[] _parameters;
    public readonly IReadOnlyList<ParameterNode> ConstantSizeParameters;

    public int Count { get; }

    public ParameterNode this[int index] => _parameters[index];

    public ParameterCollectionNode(ImmutableArray<ParameterInfo> parameterInfos, TypeNodeCache typeNodeCache)
    {
        Count = parameterInfos.Length;
        _parameters = new ParameterNode[Count];
        List<ParameterNode> constantSizeParameters = new(Count);
        for (int i = 0; i < Count; i++)
        {
            ParameterInfo parameterInfo = parameterInfos[i];
            TypeNode type = typeNodeCache.GetOrAdd(parameterInfo.Type);

            var parameter = new ParameterNode(parameterInfo.Name, i, type);

            bool isConstantSize = parameter.Type.IsConstantSize;

            _parameters[i] = parameter;

            if (isConstantSize)
            {
                constantSizeParameters.Add(parameter);
            }
            //todo: add dynamic size parameters
        }

        ConstantSizeParameters = constantSizeParameters;
    }

    public IEnumerator<ParameterNode> GetEnumerator() => ((IEnumerable<ParameterNode>)_parameters).GetEnumerator();

    public string GetValueArgumentsString() => string.Join(", ", this.Select(parameter => parameter.ArgumentVariableName));

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
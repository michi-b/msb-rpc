using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using MsbRpc.Generator.Info;
using MsbRpc.Generator.Serialization;

namespace MsbRpc.Generator.GenerationTree;

public class ParameterCollectionNode : IReadOnlyList<ParameterNode>
{
    private readonly ParameterNode[] _parameters;

    public int Count { get; }
    public bool IsAnySerializable => _parameters.Any(parameter => parameter.Serialization.GetCanHandleRpcArguments());

    public ParameterNode this[int index] => _parameters[index];

    public ParameterCollectionNode(ImmutableArray<ParameterInfo> parameterInfos, SerializationResolver serializationResolver)
    {
        Count = parameterInfos.Length;
        _parameters = new ParameterNode[Count];
        for (int i = 0; i < Count; i++)
        {
            ParameterInfo parameterInfo = parameterInfos[i];

            ISerialization serialization = serializationResolver.Resolve(parameterInfo.TypeReference);

            var parameter = new ParameterNode(parameterInfo.Name, i, serialization);

            _parameters[i] = parameter;
        }
    }

    public IEnumerator<ParameterNode> GetEnumerator() => ((IEnumerable<ParameterNode>)_parameters).GetEnumerator();

    public string GetAllValueArgumentsString() => string.Join(", ", this.Select(parameter => parameter.ArgumentVariableName));

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
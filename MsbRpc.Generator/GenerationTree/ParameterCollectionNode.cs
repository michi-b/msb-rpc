using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using MsbRpc.Generator.Extensions;
using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.GenerationTree;

internal class ParameterCollectionNode : IReadOnlyList<ParameterNode>
{
    private readonly ParameterNode[] _parameters;
    public readonly IReadOnlyList<ParameterNode> ConstantSizeParameters;
    public readonly bool HasOnlyConstantSizeParameters = true;
    public readonly bool IsValid;
    public readonly int LastIndex;

    public int Count { get; }

    public ParameterNode this[int index] => _parameters[index];

    public ParameterCollectionNode(ImmutableArray<ParameterInfo> parameterInfos, TypeNodeCache typeNodeCache, SourceProductionContext context)
    {
        Count = parameterInfos.Length;
        LastIndex = Count - 1;
        _parameters = new ParameterNode[Count];
        List<ParameterNode> constantSizeParameters = new(Count);
        IsValid = true;
        for (int i = 0; i < Count; i++)
        {
            ParameterInfo parameterInfo = parameterInfos[i];
            TypeNode type = typeNodeCache.GetOrAdd(parameterInfo.Type);

            if (!type.IsValidParameter)
            {
                context.ReportTypeIsNotAValidRpcParameter(type);
                IsValid = false;
            }

            var parameter = new ParameterNode(parameterInfo.Name, type);

            bool isConstantSize = parameter.Type.IsConstantSize;
            HasOnlyConstantSizeParameters = HasOnlyConstantSizeParameters && isConstantSize;

            _parameters[i] = parameter;

            if (isConstantSize)
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

    public IEnumerator<ParameterNode> GetEnumerator() => ((IEnumerable<ParameterNode>)_parameters).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
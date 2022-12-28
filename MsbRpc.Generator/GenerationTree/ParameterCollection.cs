using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using MsbRpc.Generator.Extensions;
using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.GenerationTree;

internal class ParameterCollection : IReadOnlyList<Parameter>
{
    private readonly Parameter[] _parameters;
    public readonly IReadOnlyList<Parameter> ConstantSizeParameters;
    public readonly bool HasOnlyConstantSizeParameters = true;
    public readonly bool IsValid;
    public readonly int LastIndex;

    public int Count { get; }

    public Parameter this[int index] => _parameters[index];

    public ParameterCollection(ImmutableArray<ParameterInfo> parameterInfos, TypeNodeCache typeNodeCache, SourceProductionContext context)
    {
        Count = parameterInfos.Length;
        LastIndex = Count - 1;
        _parameters = new Parameter[Count];
        List<Parameter> constantSizeParameters = new(Count);
        IsValid = true;
        for (int i = 0; i < Count; i++)
        {
            ParameterInfo parameterInfo = parameterInfos[i];
            TypeNode type = typeNodeCache.GetOrAdd(parameterInfo.Type, context);

            if (!type.IsValidParameter)
            {
                context.ReportTypeIsNotAValidRpcParameter(type);
                IsValid = false;
            }

            var parameter = new Parameter(parameterInfo.Name, type);

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

    public IEnumerator<Parameter> GetEnumerator() => ((IEnumerable<Parameter>)_parameters).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
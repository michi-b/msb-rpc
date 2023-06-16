using System.Collections.Immutable;
using MsbRpc.Generator.Info;
using MsbRpc.Generator.Serialization;

namespace MsbRpc.Generator.GenerationTree;

internal class ProcedureNode
{
    public readonly string FullName;
    public readonly bool HasParameters;

    public readonly string Name;
    public readonly ParameterCollectionNode? Parameters;
    public readonly string ProcedureEnumIntValue;

    public readonly string ProcedureEnumValue;
    public readonly ISerialization ResultSerialization;

    public ProcedureNode
    (
        ProcedureInfo info,
        ProcedureCollectionNode collectionNode,
        int definitionIndex,
        SerializationResolver serializationResolver
    )
    {
        Name = info.Name;
        FullName = $"{collectionNode.ProcedureEnumType}.{Name}";
        ProcedureEnumValue = $"{collectionNode.ProcedureEnumName}.{Name}";
        ProcedureEnumIntValue = definitionIndex.ToString();

        ResultSerialization = serializationResolver.Resolve(info.ResultType);

        ImmutableArray<ParameterInfo> parameterInfos = info.Parameters;
        HasParameters = parameterInfos.Length > 0;
        Parameters = HasParameters ? new ParameterCollectionNode(parameterInfos, serializationResolver) : null;
    }
}
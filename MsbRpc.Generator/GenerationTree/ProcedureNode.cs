using System.Collections.Immutable;
using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.GenerationTree;

internal class ProcedureNode
{
    public readonly ProcedureCollectionNode CollectionNode;

    public readonly string FullName;
    public readonly bool HasParameters;
    public readonly bool HasReturnValue;

    public readonly string Name;
    public readonly ParameterCollectionNode? Parameters;
    public readonly string ProcedureEnumIntValue;

    public readonly string ProcedureEnumValue;
    public readonly TypeNode ReturnType;

    public ProcedureNode
    (
        ProcedureInfo info,
        ProcedureCollectionNode collectionNode,
        int definitionIndex,
        TypeNodeCache typeNodeCache
    )
    {
        CollectionNode = collectionNode;

        Name = info.Name;
        FullName = $"{collectionNode.ProcedureEnumType}.{Name}";
        ProcedureEnumValue = $"{collectionNode.ProcedureEnumName}.{Name}";
        ProcedureEnumIntValue = definitionIndex.ToString();

        ReturnType = typeNodeCache.GetOrAdd(info.ReturnType);

        HasReturnValue = !ReturnType.IsVoid;

        ImmutableArray<ParameterInfo> parameterInfos = info.Parameters;
        HasParameters = parameterInfos.Length > 0;
        Parameters = HasParameters ? new ParameterCollectionNode(parameterInfos, typeNodeCache) : null;
    }
}
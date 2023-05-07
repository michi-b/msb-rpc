using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using MsbRpc.Generator.Info;
using MsbRpc.Generator.Serialization;
using static MsbRpc.Generator.Utility.IndependentNames;

namespace MsbRpc.Generator.GenerationTree;

internal class ProcedureCollectionNode : IReadOnlyList<ProcedureNode>
{
    private readonly ProcedureNode[] _procedures;
    public readonly ContractNode Contract;
    public readonly int LastIndex;
    public readonly string ProcedureEnumExtensionsName;
    public readonly string ProcedureEnumName;
    public readonly string ProcedureEnumParameter;
    public readonly string ProcedureEnumType;

    public ProcedureNode this[int index] => _procedures[index];

    public int Length { get; }
    public int Count => Length;

    public ProcedureCollectionNode
    (
        ImmutableArray<ProcedureInfo> procedures,
        ContractNode contract,
        SerializationResolver serializationResolver
    )
    {
        Contract = contract;
        ProcedureEnumName = $"{contract.PascalCaseName}{ProcedurePostfix}";
        ProcedureEnumType = $"{contract.Namespace}.{ProcedureEnumName}";
        ProcedureEnumExtensionsName = $"{ProcedureEnumName}{ExtensionsPostFix}";
        ProcedureEnumParameter = $"{ProcedureEnumName} {Parameters.Procedure}";
        Length = procedures.Length;
        LastIndex = Length - 1;

        _procedures = new ProcedureNode[Length];
        for (int i = 0; i < Length; i++)
        {
            var procedure = new ProcedureNode(procedures[i], this, i, serializationResolver);
            _procedures[i] = procedure;
        }
    }

    public IEnumerator<ProcedureNode> GetEnumerator() => ((IEnumerable<ProcedureNode>)_procedures).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
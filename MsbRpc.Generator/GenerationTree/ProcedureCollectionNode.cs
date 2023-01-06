using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using MsbRpc.Generator.Info;
using static MsbRpc.Generator.CodeWriters.Utility.IndependentNames;

namespace MsbRpc.Generator.GenerationTree;

internal class ProcedureCollectionNode : IReadOnlyList<ProcedureNode>
{
    private readonly ProcedureNode[] _procedures;
    public readonly ContractNode Contract;
    public readonly string EnumName;
    public readonly bool IsValid = true;
    public readonly int LastIndex;

    public ProcedureNode this[int index] => _procedures[index];

    public int Count { get; }

    public ProcedureCollectionNode
    (
        ImmutableArray<ProcedureInfo> procedures,
        ContractNode contract,
        TypeNodeCache typeNodeCache,
        SourceProductionContext context
    )
    {
        Contract = contract;
        EnumName = $"{contract.PascalCaseName}{ProcedurePostfix}";
        Count = procedures.Length;
        LastIndex = Count - 1;

        _procedures = new ProcedureNode[Count];
        for (int i = 0; i < Count; i++)
        {
            var procedure = new ProcedureNode(procedures[i], this, i, typeNodeCache, context);
            IsValid = IsValid && procedure.IsValid;
            _procedures[i] = procedure;
        }

        IsValid = IsValid && _procedures.Length > 0;
    }

    public IEnumerator<ProcedureNode> GetEnumerator() => ((IEnumerable<ProcedureNode>)_procedures).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
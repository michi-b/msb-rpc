using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using MsbRpc.Generator.GenerationTree.Names;
using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.GenerationTree;

internal class ProcedureCollection : IReadOnlyList<Procedure>
{
    private readonly Procedure[] _procedures;
    public readonly bool IsValid = true;
    public readonly int LastIndex;
    public readonly ProcedureCollectionNames Names;

    public Procedure this[int index] => _procedures[index];

    public int Count { get; }

    public ProcedureCollection
    (
        ImmutableArray<ProcedureInfo> procedures,
        ContractNames contractNames,
        TypeNodeCache typeNodeCache,
        SourceProductionContext context
    )
    {
        Names = new ProcedureCollectionNames(contractNames);
        Count = procedures.Length;
        LastIndex = Count - 1;

        _procedures = new Procedure[Count];
        for (int i = 0; i < Count; i++)
        {
            var procedure = new Procedure(procedures[i], Names, i, typeNodeCache, context);
            IsValid = IsValid && procedure.IsValid;
            _procedures[i] = procedure;
        }
    }

    public IEnumerator<Procedure> GetEnumerator() => ((IEnumerable<Procedure>)_procedures).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
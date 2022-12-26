using System.Collections;
using System.Collections.Immutable;
using MsbRpc.Generator.HelperTree.Names;
using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.HelperTree;

public class ProcedureCollection : IReadOnlyList<Procedure>
{
    private readonly Procedure[] _procedures;
    public readonly int LastIndex;
    public readonly ProcedureCollectionNames Names;
    public Procedure this[int index] => _procedures[index];

    public int Count { get; }

    public ProcedureCollection
    (
        ImmutableArray<ProcedureInfo> procedures,
        EndPointNames endPointNames,
        TypeCache typeCache
    )
    {
        Names = new ProcedureCollectionNames(endPointNames);
        Count = procedures.Length;
        LastIndex = Count - 1;

        _procedures = new Procedure[Count];
        for (int i = 0; i < Count; i++)
        {
            _procedures[i] = new Procedure(procedures[i], Names, i, typeCache);
        }
    }

    public IEnumerator<Procedure> GetEnumerator() => ((IEnumerable<Procedure>)_procedures).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
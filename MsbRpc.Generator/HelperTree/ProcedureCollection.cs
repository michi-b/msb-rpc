using System.Collections;
using System.Collections.Immutable;
using MsbRpc.Generator.HelperTree.Names;
using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.HelperTree;

public class ProcedureCollection : IEnumerable<Procedure>
{
    private readonly Procedure[] _procedures;
    public readonly int Length;
    public readonly int LastIndex;
    public readonly ProcedureCollectionNames Names;
    public Procedure this[int index] => _procedures[index];

    public ProcedureCollection
    (
        ImmutableArray<ProcedureInfo> procedures,
        ContractNames contractNames,
        EndPointNames endPointNames,
        TypeCache typeCache
    )
    {
        Names = new ProcedureCollectionNames(contractNames, endPointNames);
        Length = procedures.Length;
        LastIndex = Length - 1;

        _procedures = new Procedure[Length];
        for (int i = 0; i < Length; i++)
        {
            _procedures[i] = new Procedure(procedures[i], Names, i, typeCache);
        }
    }

    public IEnumerator<Procedure> GetEnumerator() => ((IEnumerable<Procedure>)_procedures).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
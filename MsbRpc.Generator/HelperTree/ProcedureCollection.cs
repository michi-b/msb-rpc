using System.Collections.Immutable;
using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.HelperTree;

public class ProcedureCollection
{
    public readonly ProcedureCollectionNames Names;
    public readonly int Count;
    public readonly int LastIndex;

    private readonly Procedure[] _procedures;
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
        Count = procedures.Length;
        LastIndex = Count - 1;
        
        _procedures = new Procedure[Count];
        for (int i = 0; i < Count; i++)
        {
            _procedures[i] = new Procedure(procedures[i], Names, i, typeCache);
        }
    }
}
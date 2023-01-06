using MsbRpc.Generator.GenerationTree.Names;

namespace MsbRpc.Generator.GenerationTree;

internal class EndPoint
{
    private readonly EndPointNames _names;
    private readonly ProcedureCollection? _procedures;

    public EndPoint
    (
        EndPointNames names,
        ProcedureCollection? procedures
    )
    {
        _names = names;
        _procedures = procedures;
    }
}
using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.GenerationTree;

internal class ServerGenerationNode
{
    public ServerGenerationInfo Info { get; }
    public ContractNode Contract { get; }

    public ServerGenerationNode(ServerGenerationInfo info, ContractNode contract)
    {
        Info = info;
        Contract = contract;
    }
}
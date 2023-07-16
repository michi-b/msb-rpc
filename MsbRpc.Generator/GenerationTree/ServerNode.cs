#region

using MsbRpc.Generator.Info;
using static MsbRpc.Generator.Utility.Names;

#endregion

namespace MsbRpc.Generator.GenerationTree;

internal class ServerNode
{
    private const string ServerPostfix = "Server";
    public ServerGenerationInfo Info { get; }
    public ContractNode Contract { get; }

    public EndPointNode EndPoint { get; }

    public string ImplementationFactoryType { get; }

    public string Name { get; }

    public string FullName { get; }

    public ServerNode(ServerGenerationInfo info, ContractNode contract)
    {
        Info = info;
        Contract = contract;
        Name = $"{Contract.PascalCaseName}{ServerPostfix}";
        FullName = $"{Contract.Namespace}.{Name}";
        ImplementationFactoryType = $"{Interfaces.Factory}<{contract.Interface}>";
        EndPoint = contract.ServerEndPoint;
    }
}
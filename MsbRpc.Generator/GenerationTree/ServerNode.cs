using System;
using MsbRpc.Generator.Info;
using static MsbRpc.Generator.CodeWriters.Utility.IndependentNames;

namespace MsbRpc.Generator.GenerationTree;

internal class ServerNode
{
    public readonly int DefaultPort;
    public readonly string EndPointConfigurationTypeFullName;
    public readonly string Name;
    public readonly EndPointNode EndPoint;
    public readonly ContractNode Contract;
    
    private ServerNode(ContractInfo info, EndPointNode serverEndPoint)
    {
        DefaultPort = info.DefaultPort;
        EndPoint = serverEndPoint;
        Contract = serverEndPoint.Contract;
        EndPointConfigurationTypeFullName = $"{Contract.Namespace}.{serverEndPoint.Name}.{Types.LocalConfiguration}";
        Name = EndPoint.NameBase;
    }


    public static ServerNode? Create(ContractInfo info, EndPointNode serverEndPoint) => info.GenerateServer ? new ServerNode(info, serverEndPoint) : null;
}
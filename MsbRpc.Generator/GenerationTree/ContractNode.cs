using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using MsbRpc.Generator.Enums;
using MsbRpc.Generator.Info;
using MsbRpc.Generator.Serialization;
using static MsbRpc.Generator.Utility.Names;

namespace MsbRpc.Generator.GenerationTree;

public class ContractNode
{
    public readonly ContractAccessibility Accessibility;

    public readonly EndPointNode ClientEndPoint;
    public readonly int DefaultInitialBufferSize;

    public readonly string InterfaceName;
    public readonly string InterfaceType;

    public readonly string Namespace;
    public readonly string PascalCaseName;
    public readonly ProcedureCollectionNode Procedures;
    public readonly EndPointNode ServerEndPoint;

    public ContractNode(ref ContractInfo info)
    {
        Accessibility = info.Accessibility;
        DefaultInitialBufferSize = info.InitialBufferSize;

        InterfaceName = info.InterfaceName;
        InterfaceType = $"{info.Namespace}.{info.InterfaceName}";
        PascalCaseName = GetContractName(InterfaceName);
        Namespace = $"{info.Namespace}{GeneratedNamespacePostFix}";

        var serializationResolver = new SerializationResolver(info.CustomSerializations.ToArray());

        ImmutableArray<ProcedureInfo> procedures = info.Procedures;

        Procedures = new ProcedureCollectionNode(procedures, this, serializationResolver);

        ClientEndPoint = CreateEndPointNode(info, EndPointType.Client);
        ServerEndPoint = CreateEndPointNode(info, EndPointType.Server);
    }

    private EndPointNode CreateEndPointNode(ContractInfo info, EndPointType endPointType)
        => new
        (
            this,
            endPointType,
            info.Direction.GetDirection(endPointType)
        );

    private static string GetContractName(string contractInterfaceName)
    {
        string iStripped = contractInterfaceName.StartsWith(InterfacePrefix, StringComparison.Ordinal)
                           && char.IsUpper(contractInterfaceName[1])
            ? contractInterfaceName.Substring(1)
            : contractInterfaceName;
        return iStripped.CamelToPascalCase();
    }
}
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using MsbRpc.Generator.Enums;
using MsbRpc.Generator.Info;
using MsbRpc.Generator.Serialization;
using static MsbRpc.Generator.CodeWriters.Utility.IndependentNames;

namespace MsbRpc.Generator.GenerationTree;

internal class ContractNode
{
    public readonly EndPointNode ClientEndPoint;

    public readonly string InterfaceName;
    public readonly string InterfaceType;

    public readonly string Namespace;
    public readonly string PascalCaseName;
    public readonly ProcedureCollectionNode Procedures;
    public readonly EndPointNode ServerEndPoint;

    public ContractNode(ref ContractInfo info)
    {
        InterfaceName = info.InterfaceName;
        InterfaceType = $"{info.Namespace}.{info.InterfaceName}";
        PascalCaseName = GetContractName(InterfaceName);
        PascalCaseName.PascalToCamelCase();
        Namespace = $"{info.Namespace}{GeneratedNamespacePostFix}";

        ImmutableArray<CustomSerializationInfo> customSerializations = info.CustomSerializations
            .Select(pair => pair.Value).ToImmutableArray();
        
        SerializationResolver serializationResolver = new SerializationResolver(customSerializations);
        
        ImmutableArray<ProcedureInfo> procedures = info.Procedures;

        Procedures = new ProcedureCollectionNode(procedures, this, serializationResolver);

        ClientEndPoint = CreateEndPointNode(info, ConnectionEndType.Client);
        ServerEndPoint = CreateEndPointNode(info, ConnectionEndType.Server);
    }

    private EndPointNode CreateEndPointNode(ContractInfo info, ConnectionEndType endType)
        => new
        (
            this,
            endType,
            info.ContractType.GetDirection(endType)
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
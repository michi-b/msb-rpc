using System;
using System.Collections.Immutable;
using System.Linq;
using MsbRpc.Generator.Enums;
using MsbRpc.Generator.Extensions;
using MsbRpc.Generator.Info;
using MsbRpc.Generator.Serialization;
using static MsbRpc.Generator.Utility.Names;

namespace MsbRpc.Generator.GenerationTree;

public class ContractNode
{
    private const string ImplementationFactoryInterfacePostFix = "ImplementationFactory";
    private const string ServerPostfix = "Server";

    public readonly string AccessibilityKeyword;

    public readonly int DefaultInitialBufferSize;

    public readonly bool GenerateServer;

    // contract name depended names
    public readonly string ImplementationFactoryInterfaceName;

    /// <summary>
    ///     interface name prepended with namespace
    /// </summary>
    public readonly string Interface;

    public readonly string InterfaceName;

    public readonly string Namespace;

    /// <summary>
    ///     pascale case name of the contract interface without the leading 'I'
    /// </summary>
    public readonly string PascalCaseName;

    // child nodes
    public readonly ProcedureCollectionNode Procedures;
    public readonly EndPointNode ServerEndPoint;
    public readonly EndPointNode ClientEndPoint;

    /// <summary>
    ///     name of the server class
    /// </summary>
    public readonly string ServerName;

    /// <summary>
    ///     server name prepended with namespace
    /// </summary>
    public readonly string ServerType;

    public ContractNode(ref ContractInfo info)
    {
        AccessibilityKeyword = info.Accessibility.GetKeyword();
        
        DefaultInitialBufferSize = info.InitialBufferSize;
        GenerateServer = info.GenerateServer;

        InterfaceName = info.InterfaceName;
        Interface = $"{info.Namespace}.{info.InterfaceName}";
        PascalCaseName = GetContractName(InterfaceName);
        Namespace = $"{info.Namespace}{GeneratedNamespacePostFix}";

        //contract name depended names
        ImplementationFactoryInterfaceName = $"{InterfaceName}{ImplementationFactoryInterfacePostFix}";
        ServerName = $"{PascalCaseName}{ServerPostfix}";
        ServerType = $"{Namespace}.{ServerName}";

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
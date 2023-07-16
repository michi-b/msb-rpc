#region

using System;

#endregion

namespace MsbRpc.Generator.Attributes;

/// <summary>
///     Instructs a server implementation to be generated for a contract.
///     This attribute is therefore only valid on interfaces marked with <see cref="RpcContractAttribute" />.
///     A server in this context is a class that listens for new connections serve as RPC channels of the target RPC
///     contract.
///     There are other ways to create such RPC channels, but usually on "entry point" contract will generate a server,
///     that can then be used to create further RPC channels.
/// </summary>
[GeneratorMarker]
[AttributeUsage(AttributeTargets.Interface)]
public class GenerateServerAttribute : Attribute { }
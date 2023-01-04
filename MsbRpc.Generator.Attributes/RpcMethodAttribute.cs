using System;

namespace MsbRpc.Generator.Attributes;

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public class RpcMethodAttribute : Attribute
{
    public readonly bool DiscontinuesCommunication;

    public RpcMethodAttribute(bool discontinuesCommunication = false) => DiscontinuesCommunication = discontinuesCommunication;
}
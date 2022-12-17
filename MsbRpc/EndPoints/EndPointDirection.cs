using JetBrains.Annotations;
using MsbRpc.Utility;

namespace MsbRpc.EndPoints;

[PublicAPI(Messages.GeneratorTarget)]
public enum EndPointDirection
{
    Inbound,
    Outbound
}
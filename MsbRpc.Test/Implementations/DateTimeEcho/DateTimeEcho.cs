using MsbRpc.Contracts;

namespace MsbRpc.Test.Implementations.DateTimeEcho;

public interface IEcho : IRpcContract
{
    System.DateTime GetDateTime(System.DateTime myDateTime);
}
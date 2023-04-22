using MsbRpc.Contracts;

namespace MsbRpc.Test.Implementations.DateTimeEcho;

public interface IDateTimeEcho : IRpcContract
{
    DateTime GetDateTime(DateTime myDateTime);
}
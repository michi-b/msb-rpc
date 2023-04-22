using MsbRpc.Contracts;

namespace MsbRpc.Test.Implementations.DateTimeEcho;

public class DateTimeEcho : RpcContractImplementation, IDateTimeEcho
{
    public DateTime GetDateTime(DateTime myDateTime) => DateTime.Now;
}
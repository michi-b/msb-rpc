#region

using MsbRpc.Contracts;

#endregion

namespace MsbRpc.Test.Implementations.DateTimeEcho;

public interface IDateTimeEcho : IRpcContract
{
    DateTime GetDateTime(DateTime clientDateTime);
}
using MsbRpc.Contracts;

namespace MsbRpc.Test.Implementations.DateTimeEcho;

public class DateTimeEcho : RpcContractImplementation, IDateTimeEcho
{
    private const int RetryMillisecondsTimeout = 10;

    public DateTime GetDateTime(DateTime clientDateTime)
    {
        DateTime result;
        do
        {
            Thread.Sleep(RetryMillisecondsTimeout);
            result = DateTime.Now;
        } while (result == clientDateTime);

        MarkRanToCompletion();

        return result;
    }

    protected override void Dispose(bool disposing)
    {
        // Nothing to dispose
    }
}
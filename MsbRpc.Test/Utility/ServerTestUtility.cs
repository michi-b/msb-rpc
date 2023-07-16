#region

using System.Net;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Configuration;
using MsbRpc.Configuration.Builders;

#endregion

namespace MsbRpc.Test.Utility;

public static class ServerTestUtility
{
    private const int MaxRetries = 100;
    private const int RetryTimeOut = 1;

    public static readonly IPAddress LocalHost = Dns.GetHostEntry("localhost").AddressList[0];

    public static readonly OutboundEndPointConfiguration ClientEndPointConfiguration =
        new OutboundEndPointConfigurationBuilder { LoggerFactory = TestUtility.LoggerFactory }.Build();

    public static async ValueTask AssertBecomesEqual<T>(T expected, Func<T> getActual, string? message = null) where T : IEquatable<T>
    {
        //default is never used because RetryCount is > 0, but the compiler still complains without the assignment
        T actual = default!;
        bool timedOut = true;
        int retryCount;
        for (retryCount = 0; retryCount < MaxRetries; retryCount++)
        {
            actual = getActual();
            if (actual.Equals(expected))
            {
                timedOut = false;
                break;
            }

            await Task.Delay(RetryTimeOut);
        }

        if (timedOut)
        {
            Assert.IsFalse(timedOut, $"{nameof(AssertBecomesEqual)} timed out; expected {expected} is actually {actual} at this moment");
        }
        else
        {
            TestUtility.Logger.LogInformation("{AssertionMethodName} succeeded after {RetryCount} retries", nameof(AssertBecomesEqual), retryCount);
        }

        Assert.AreEqual(expected, actual, message);
    }
}
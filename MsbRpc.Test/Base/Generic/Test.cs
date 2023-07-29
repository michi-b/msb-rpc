#region

using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using MsbRpc.Test.Utility;

#endregion

namespace MsbRpc.Test.Base.Generic;

public class Test<TTest> : Test where TTest : Test<TTest>
{
    [PublicAPI] protected static readonly ILogger<TTest> Logger = CreateLogger();

    [PublicAPI] public static ILoggerFactory LoggerFactory => TestUtility.LoggerFactory;

    private static ILogger<TTest> CreateLogger() => LoggerFactory.CreateLogger<TTest>();
}
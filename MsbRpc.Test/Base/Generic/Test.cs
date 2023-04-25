using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using MsbRpc.Test.Utility;

namespace MsbRpc.Test.Base.Generic;

public class Test<TTest> : Test where TTest : Test<TTest>
{
    [PublicAPI] protected static readonly ILogger<TTest> Logger;

    static Test() => Logger = TestUtility.LoggerFactory.CreateLogger<TTest>();
}
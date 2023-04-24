using JetBrains.Annotations;
using MsbRpc.Configuration;

namespace MsbRpc.Test.Base.Generic;

public class ServerTest<TTest> : Test<TTest> where TTest : ServerTest<TTest>
{
    [PublicAPI]
    protected ServerConfiguration Configuration { get; }

    public ServerTest(ServerConfiguration serverConfiguration) => Configuration = serverConfiguration;
    
    
}
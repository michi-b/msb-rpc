using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MsbRpcTest;

public class Test
{
    // ReSharper disable once MemberCanBePrivate.Global
    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
    // MSTest needs the public setter
    public TestContext TestContext { private get; set; } = null!;

    protected CancellationToken CancellationToken => TestContext.CancellationTokenSource.Token;
}
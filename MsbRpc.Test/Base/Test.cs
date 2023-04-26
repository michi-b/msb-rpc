using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MsbRpc.Test.Base;

public class Test
{
    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
    // ReSharper disable once MemberCanBeProtected.Global
    // MSTest needs the public setter
    public TestContext TestContext { protected get; set; } = null!;

    [PublicAPI] protected CancellationToken CancellationToken => TestContext.CancellationTokenSource.Token;
}
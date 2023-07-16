#region

using System.Threading;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace MsbRpc.Test.Generator.Base;

public class Test
{
    // ReSharper disable once MemberCanBePrivate.Global
    // ReSharper disable once MemberCanBeProtected.Global
    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
    // MSTest needs the public setter
    public TestContext TestContext { protected get; set; } = null!;

    [PublicAPI] protected CancellationToken CancellationToken => TestContext.CancellationTokenSource.Token;
}
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Misbat.CodeAnalysis.Test.CodeTest;
using Serilog;
using Serilog.Core;

namespace MsbRpc.Test.Generator;

public abstract class SingleSourceGeneratorTest<TTest> : Test
    where TTest : SingleSourceGeneratorTest<TTest>
{
    private readonly CodeTest _codeTest;
    private readonly ILoggerFactory _loggerFactory;

    [PublicAPI] protected ILogger<TTest> Logger;

    protected abstract string Code { get; }

    protected abstract string Namespace { get; }

    protected SingleSourceGeneratorTest()
    {
        Logger logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .CreateLogger()!;

        _loggerFactory = new LoggerFactory().AddSerilog(logger);

        Logger = _loggerFactory.CreateLogger<TTest>();

        // ReSharper disable twice VirtualMemberCallInConstructor
        // yes, this is bad practice, derived types must make sure not to have these overrides rely on the constructor being called
        _codeTest = CodeTestUtility.GetContractGeneratorCodeTest<TTest>(Code, Namespace);
    }

    protected async Task<CodeTestResult> RunCodeTest(CodeTest.LoggingOptions loggingOptions = CodeTest.LoggingOptions.None)
        => (await _codeTest.Run(CancellationToken, _loggerFactory, loggingOptions)).Result;
}
using System;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Misbat.CodeAnalysis.Test.TestBases;
using MsbRpc.Contracts;
using MsbRpc.Generator.Attributes;
using MsbRpc.Test.Generator.Utility;
using Serilog;
using Serilog.Core;

namespace MsbRpc.Test.Generator.Base;

public abstract class ContractGenerationTest<TTest, TGenerator> : SingleGenerationTest<TTest, TGenerator>
    where TTest : ContractGenerationTest<TTest, TGenerator>, new()
    where TGenerator : IIncrementalGenerator, new()
{
    // ReSharper disable once StaticMemberInGenericType
    // yeah let's ignore this
    private static readonly Type[] StaticReferencedTypes = { typeof(RpcContractAttribute), typeof(IRpcContract) };

    protected ContractGenerationTest(string code, string nameSpace) : base
    (
        CreateLoggerFactory(),
        d => d.Severity != DiagnosticSeverity.Hidden,
        codeTest => CodeTestUtility.Configure(codeTest).WithCode(code).InNamespace(nameSpace),
        StaticReferencedTypes
    ) { }

    private static ILoggerFactory CreateLoggerFactory()
    {
        Logger logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.Console()
            .CreateLogger()!;
        return new LoggerFactory().AddSerilog(logger);
    }
}
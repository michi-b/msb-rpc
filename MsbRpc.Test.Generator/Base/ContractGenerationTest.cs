using System;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Misbat.CodeAnalysis.Test.CodeTest;
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
    private readonly string _contractName;

    protected ContractGenerationTest(string code, string nameSpace, string contractName) : base
    (
        CreateLoggerFactory(),
        d => d.Severity != DiagnosticSeverity.Hidden,
        codeTest => CodeTestUtility.Configure(codeTest).WithCode(code).InNamespace(nameSpace),
        StaticReferencedTypes
    )
        => _contractName = contractName;

    private static ILoggerFactory CreateLoggerFactory()
    {
        Logger logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.Console()
            .CreateLogger()!;
        return new LoggerFactory().AddSerilog(logger);
    }

    #region FileGeneration

    protected async Task<CodeTestResult> TestGeneratesClientEndPoint() => await TestGeneratesFile($"{_contractName}ClientEndPoint.g.cs");
    protected async Task<CodeTestResult> TestGeneratesServerEndPoint() => await TestGeneratesFile($"{_contractName}ServerEndPoint.g.cs");
    protected async Task<CodeTestResult> TestGeneratesProcedureEnum() => await TestGeneratesFile($"{_contractName}Procedure.g.cs");
    protected async Task<CodeTestResult> TestGeneratesProcedureEnumExtensions() => await TestGeneratesFile($"{_contractName}ProcedureExtensions.g.cs");

    protected async Task<CodeTestResult> TestGeneratesClientEndPointConfigurationBuilder()
        => await TestGeneratesFile($"{_contractName}ClientEndPointConfigurationBuilder.g.cs");

    protected async Task<CodeTestResult> TestGeneratesServerEndPointConfigurationBuilder()
        => await TestGeneratesFile($"{_contractName}ServerEndPointConfigurationBuilder.g.cs");

    protected async Task<CodeTestResult> TestGeneratesServerConfigurationBuilder() => await TestGeneratesFile($"{_contractName}ServerConfigurationBuilder.g.cs");

    protected async Task<CodeTestResult> TestGeneratesServer() => await TestGeneratesFile($"{_contractName}Server.g.cs");

    #endregion
}
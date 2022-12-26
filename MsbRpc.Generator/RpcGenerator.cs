//todo: implement

using System.Runtime.CompilerServices;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using MsbRpc.Generator.CodeWriters;
using MsbRpc.Generator.HelperTree;
using MsbRpc.Generator.Info;
using MsbRpc.Generator.Info.Comparers;
using MsbRpc.Generator.Utility;

namespace MsbRpc.Generator;

[Generator]
public class Generator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        IncrementalValuesProvider<ContractInfo> rpcContractDeclarationSyntaxNodes = context.SyntaxProvider.CreateSyntaxProvider
            (
                GetIsAttributedInterfaceDeclarationSyntax,
                GetContractInfo
            )
            .Where(contract => contract != null)
            .Select((contractInfo, _) => (ContractInfo)contractInfo!);

        IncrementalValuesProvider<ContractInfo> rpcContracts = rpcContractDeclarationSyntaxNodes
            .Collect()
            .SelectMany((infos, _) => infos.Distinct(TargetComparer.Instance));

        context.RegisterSourceOutput(rpcContracts, Generate);
    }

    private static bool GetIsAttributedInterfaceDeclarationSyntax(SyntaxNode syntaxNode, CancellationToken cancellationToken)
    {
        var interfaceDeclarationSyntax = syntaxNode as InterfaceDeclarationSyntax;
        return interfaceDeclarationSyntax != null && interfaceDeclarationSyntax.AttributeLists.Any();
    }

    private static ContractInfo? GetContractInfo(GeneratorSyntaxContext context, CancellationToken cancellationToken)
    {
        SemanticModel semanticModel = context.SemanticModel;

        // get interface symbol
        if (semanticModel.GetDeclaredSymbol(context.Node, cancellationToken) is not INamedTypeSymbol contractInterface)
        {
            return null;
        }

        // check that interfaceSymbol is actually an interface
        if (contractInterface.TypeKind != TypeKind.Interface)
        {
            return null;
        }

        // check that interfaceSymbol has the RpcContract attribute
        return contractInterface.GetAttributes()
            .Select(attributeData => attributeData.AttributeClass!)
            .Any(WellKnownAttributes.IsRpcContractAttribute)
            ? new ContractInfo(contractInterface)
            : null;
    }

    private static void Generate(SourceProductionContext context, ContractInfo contractInfo)
    {
        ContractNode helperTree = new(ref contractInfo);
        Task serverGenerationTask = GenerateEndPoint(context, helperTree.Server);
        Task clientGenerationTask = GenerateEndPoint(context, helperTree.Client);
        // Task.WaitAll(serverGenerationTask, clientGenerationTask);
        
        // ContractNames contractNames = new ContractNames(contractInfo);
        // var generator = new ContractGenerator(ref contractInfo);
        // GenerateEndPoint(context, generator, EndPointTypeId.Client);
        // GenerateEndPoint(context, generator, EndPointTypeId.Server);
    }

    private static async Task GenerateEndPoint(SourceProductionContext context, EndPoint endPoint)
    {
        List<Task> codeGenerationTasks = new(7);
        
        //generate inbound procedure enum and extensions and interface
        if (endPoint.TryGetInboundProcedures(out ProcedureCollection? inboundProcedures) && inboundProcedures != null)
        {
            codeGenerationTasks.Add(new ProcedureEnumWriter(endPoint.Contract, inboundProcedures).Generate(context));
            codeGenerationTasks.Add(new ProcedureEnumExtensionsWriter(endPoint.Contract, inboundProcedures).Generate(context));
            codeGenerationTasks.Add(new InterfaceWriter(endPoint.Contract, endPoint, inboundProcedures).Generate(context));
        }
        
        //generate endpoint
        endPoint.TryGetOutboundProcedures(out ProcedureCollection? outboundProcedures);
        if(inboundProcedures != null || outboundProcedures != null)
        {
            codeGenerationTasks.Add(new EndPointWriter(endPoint.Contract, endPoint, inboundProcedures, outboundProcedures).Generate(context));
        }
        
        await Task.WhenAll(codeGenerationTasks.ToArray());
    }

    // private static void GenerateEndPoint(SourceProductionContext context, ContractGenerator generator, EndPointTypeId targetEndPointType)
    // {
    //     
    //     EndPointGenerator endPointGenerator = generator[targetEndPointType];
    //
    //     if (generator[targetEndPointType].HasInboundProcedures)
    //     {
    //         context.AddSource(endPointGenerator.Names.InboundProcedureEnumFile, generator.GenerateProcedureEnum(targetEndPointType));
    //         context.AddSource
    //             (endPointGenerator.Names.InboundProcedureEnumExtensionsFile, generator.GenerateProcedureEnumExtensions(targetEndPointType));
    //         context.AddSource(endPointGenerator.Names.InterfaceFile, generator.GenerateInterface(targetEndPointType));
    //     }
    //
    //     context.AddSource(endPointGenerator.Names.EndPointFile, generator.GenerateEndPoint(targetEndPointType));
    // }
}
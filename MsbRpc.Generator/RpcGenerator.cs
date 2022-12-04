//todo: implement

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MsbRpc.Generator;

[Generator]
public class RpcGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        IncrementalValuesProvider<InterfaceDeclarationSyntax> rpcContracts
            = context.SyntaxProvider.CreateSyntaxProvider
                (
                    GetIsAttributedSyntax,
                    GetRpcContract
                )
                .Where(syntax => syntax != null)!;
        
        //need to register codegen to have the filters run
        context.RegisterSourceOutput(rpcContracts, Execute);
    }

    private static void Execute(SourceProductionContext context, InterfaceDeclarationSyntax rpcContract)
    {
        string name = rpcContract.Identifier.Text;
        Console.WriteLine($"Generating {name}");
    }

    private bool GetIsAttributedSyntax(SyntaxNode syntaxNode, CancellationToken cancellationToken)
    {
        // ReSharper disable once UsePatternMatching
        // prefer this separate for debugging
        var interfaceDeclarationSyntax = syntaxNode as InterfaceDeclarationSyntax;
        return interfaceDeclarationSyntax != null && interfaceDeclarationSyntax.AttributeLists.Any();
    }

    private static InterfaceDeclarationSyntax? GetRpcContract(GeneratorSyntaxContext context, CancellationToken cancellationToken)
    {
        context.SemanticModel.GetSymbolInfo(context.Node, cancellationToken);
        return context.Node as InterfaceDeclarationSyntax;
    }
}
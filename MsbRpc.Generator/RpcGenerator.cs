using System.CodeDom.Compiler;
using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using MsbRpc.Generator.Extensions;

namespace MsbRpc.Generator;

[Generator]
public class RpcGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        IncrementalValuesProvider<TypeDeclarationSyntax> constantSizeSerializables = context.SyntaxProvider
            .CreateSyntaxProvider(GetIsTypeDeclarationWithAttributes, SelectMsbRpsObjectDeclarationSyntax)
            .Where(syntax => syntax != null)!;

        IncrementalValueProvider<(Compilation compilation, ImmutableArray<TypeDeclarationSyntax> typeDeclarationSyntax)> compilationAndTypes
            = context.CompilationProvider.Combine(constantSizeSerializables.Collect());

        context.RegisterSourceOutput(compilationAndTypes, Execute);
    }

    private static bool GetIsTypeDeclarationWithAttributes(SyntaxNode syntaxNode, CancellationToken cancellationToken)
    {
        // ReSharper disable once MergeIntoPattern
        // I find this better readable
        return syntaxNode is TypeDeclarationSyntax typeDeclarationSyntax
               && typeDeclarationSyntax.AttributeLists.Any((attributeList) => attributeList.ChildNodes().Any(node => node is AttributeSyntax));
    }

    private static TypeDeclarationSyntax? SelectMsbRpsObjectDeclarationSyntax(GeneratorSyntaxContext context, CancellationToken cancellationToken)
    {
        SemanticModel semanticModel = context.SemanticModel;

        var syntax = (TypeDeclarationSyntax)context.Node;

        var symbol = (INamedTypeSymbol)semanticModel.GetDeclaredSymbol(context.Node)!;

        return symbol.GetAttributes()
            .Any(attribute => attribute.AttributeClass != null && attribute.AttributeClass.HasName("AttributeName"))//todo: filter for correct attribute name
            ? syntax
            : null;
    }

    private void Execute
    (
        SourceProductionContext sourceProductionContext,
        (Compilation compilation, ImmutableArray<TypeDeclarationSyntax> typeDeclarationSyntax) source
    )
    {
        ImmutableArray<TypeDeclarationSyntax> typeDeclarationSyntax = source.typeDeclarationSyntax;

        foreach (TypeDeclarationSyntax currentTypeDeclarationSyntax in typeDeclarationSyntax)
        {
            string serializationCode = GenerateSerializationCode(source.compilation, currentTypeDeclarationSyntax);
            //todo: correct filename
            //string filename = currentTypeDeclarationSyntax.ChildTokens().First(token => token.IsKind(SyntaxKind.IdentifierToken).ToString())
            sourceProductionContext.AddSource("yadayada.g.cs", SourceText.From(serializationCode, Encoding.UTF8));
        }
    }

    private string GenerateSerializationCode(Compilation sourceCompilation, TypeDeclarationSyntax currentTypeDeclarationSyntax)
    {
        TextWriter stringWriter = new StringWriter();
        var writer = new IndentedTextWriter(stringWriter);
        writer.WriteLine("using MsbRps.Interfaces;");
        return stringWriter.ToString();
    }
}
using System.CodeDom.Compiler;
using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using MsbRps.CodeAnalysis.Extensions;

namespace MsbRps.CodeAnalysis;

[Generator]
public class SerializableGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        IncrementalValuesProvider<TypeDeclarationSyntax> rpsSerializableTypeDeclarations = context.SyntaxProvider
            .CreateSyntaxProvider(GetIsTypeDeclarationSyntaxWithSimpleBase, SelectRpsSerializableTypes)
            .Where(syntax => syntax != null)!;

        IncrementalValueProvider<(Compilation compilation, ImmutableArray<TypeDeclarationSyntax> typeDeclarationSyntax)> compilationAndTypes
            = context.CompilationProvider.Combine(rpsSerializableTypeDeclarations.Collect());

        context.RegisterSourceOutput(compilationAndTypes, Execute);
    }

    private static bool GetIsTypeDeclarationSyntaxWithSimpleBase(SyntaxNode syntaxNode, CancellationToken cancellationToken)
    {
        // ReSharper disable once MergeIntoPattern
        // I find this better readable
        return syntaxNode is TypeDeclarationSyntax typeDeclarationSyntax
               && typeDeclarationSyntax.BaseList != null
               && typeDeclarationSyntax.BaseList
                   .ChildNodes()
                   .Any(node => node is SimpleBaseTypeSyntax);
    }

    private static TypeDeclarationSyntax? SelectRpsSerializableTypes(GeneratorSyntaxContext context, CancellationToken cancellationToken)
    {
        SemanticModel semanticModel = context.SemanticModel;

        var syntax = (TypeDeclarationSyntax)context.Node;

        IEnumerable<SimpleBaseTypeSyntax> baseTypeSyntax = syntax.BaseList!.ChildNodes()
            .OfType<SimpleBaseTypeSyntax>();

        var symbol = (INamedTypeSymbol)semanticModel.GetDeclaredSymbol(context.Node)!;

        if (!symbol.Interfaces.Any(interfaceSymbol => interfaceSymbol.HasName(SymbolNames.SerializableInterface)))
        {
            return null;
        }

        //return null if serialization or deserialization is implemented
        //if (symbol.GetMembers("Serialize").Any() || symbol.GetMembers("Deserialize").Any())
        //{
        //    return null;
        //}

        return syntax;
        //todo: also return symbol
        //return new ValueTuple<TypeDeclarationSyntax, INamedTypeSymbol>(syntax, symbol);
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
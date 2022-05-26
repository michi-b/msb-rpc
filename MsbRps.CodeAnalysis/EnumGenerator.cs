using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace MsbRps.CodeAnalysis;

[Generator]
public class EnumGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput
            (ctx => ctx.AddSource("EnumExtensionsAttribute.g.cs", SourceText.From(SourceGenerationHelper.Attribute, Encoding.UTF8)));

        IncrementalValuesProvider<EnumDeclarationSyntax> enumDeclarations = context.SyntaxProvider.CreateSyntaxProvider
            (
                static (s, _) => IsSyntaxTargetForGeneration(s),
                static (ctx, _) => GetSemanticTargetForGeneration(ctx)
            )
            .Where(static m => m is not null)!;

        IncrementalValueProvider<(Compilation Compilation, ImmutableArray<EnumDeclarationSyntax> enumDeclarations)> compilationAndEnums =
            context.CompilationProvider.Combine(enumDeclarations.Collect());

        context.RegisterSourceOutput(compilationAndEnums, static (spc, source) => Execute(source.Compilation, source.enumDeclarations, spc));
    }

    private static void Execute(Compilation compilation, ImmutableArray<EnumDeclarationSyntax> enums, SourceProductionContext context)
    {
        if (enums.IsDefaultOrEmpty)
        {
            // nothing to do yet
            return;
        }

        // I'm not sure if this is actually necessary, but `[LoggerMessage]` does it, so seems like a good idea!
        IEnumerable<EnumDeclarationSyntax> distinctEnums = enums.Distinct();

        // Convert each EnumDeclarationSyntax to an EnumToGenerate
        List<EnumToGenerate> enumsToGenerate = GetTypesToGenerate(compilation, distinctEnums, context.CancellationToken);

        // If there were errors in the EnumDeclarationSyntax, we won't create an
        // EnumToGenerate for it, so make sure we have something to generate
        if (enumsToGenerate.Count > 0)
        {
            // generate the source code and add it to the output
            string result = GenerateExtensionClass(enumsToGenerate);
            context.AddSource("EnumExtensions.g.cs", SourceText.From(result, Encoding.UTF8));
        }
    }

    private static EnumDeclarationSyntax? GetSemanticTargetForGeneration(GeneratorSyntaxContext context)
    {
        // we know the node is a EnumDeclarationSyntax thanks to IsSyntaxTargetForGeneration
        var enumDeclarationSyntax = (EnumDeclarationSyntax)context.Node;

        // loop through all the attributes on the method
        foreach (AttributeSyntax attributeSyntax in enumDeclarationSyntax.AttributeLists.SelectMany
                     (attributeListSyntax => attributeListSyntax.Attributes))
        {
            if (context.SemanticModel.GetSymbolInfo(attributeSyntax).Symbol is not IMethodSymbol attributeSymbol)
            {
                // weird, we couldn't get the symbol, ignore it
                continue;
            }

            INamedTypeSymbol attributeContainingTypeSymbol = attributeSymbol.ContainingType;
            string fullName = attributeContainingTypeSymbol.ToDisplayString();

            // Is the attribute the [EnumExtensions] attribute?
            if (fullName == "NetEscapades.EnumGenerators.EnumExtensionsAttribute")
            {
                // return the enum
                return enumDeclarationSyntax;
            }
        }

        return null;
    }

    private static List<EnumToGenerate> GetTypesToGenerate(Compilation compilation, IEnumerable<EnumDeclarationSyntax> enums, CancellationToken ct)
    {
        // Create a list to hold our output
        var enumsToGenerate = new List<EnumToGenerate>();
        // Get the semantic representation of our marker attribute 
        INamedTypeSymbol? enumAttribute = compilation.GetTypeByMetadataName("NetEscapades.EnumGenerators.EnumExtensionsAttribute");

        if (enumAttribute == null)
        {
            // If this is null, the compilation couldn't find the marker attribute type
            // which suggests there's something very wrong! Bail out..
            return enumsToGenerate;
        }

        foreach (EnumDeclarationSyntax enumDeclarationSyntax in enums)
        {
            // stop if we're asked to
            ct.ThrowIfCancellationRequested();

            // Get the semantic representation of the enum syntax
            SemanticModel semanticModel = compilation.GetSemanticModel(enumDeclarationSyntax.SyntaxTree);
            if (semanticModel.GetDeclaredSymbol(enumDeclarationSyntax) is not INamedTypeSymbol enumSymbol)
            {
                // something went wrong, bail out
                continue;
            }

            // Get the full type name of the enum e.g. Color, 
            // or OuterClass<T>.Color if it was nested in a generic type (for example)
            string enumName = enumSymbol.ToString();

            // Get all the members in the enum
            ImmutableArray<ISymbol> enumMembers = enumSymbol.GetMembers();
            var members = new List<string>(enumMembers.Length);

            members.AddRange(from member in enumMembers where member is IFieldSymbol { ConstantValue: { } } select member.Name);

            // Get all the fields from the enum, and add their name to the list

            // Create an EnumToGenerate for use in the generation phase
            enumsToGenerate.Add(new EnumToGenerate(enumName, members));
        }

        return enumsToGenerate;
    }

    private static bool IsSyntaxTargetForGeneration(SyntaxNode node) => node is EnumDeclarationSyntax { AttributeLists.Count: > 0 };

    private static string GenerateExtensionClass(List<EnumToGenerate> enumsToGenerate)
    {
        var sb = new StringBuilder();
        sb.Append
        (
            @"
namespace NetEscapades.EnumGenerators
{
    public static partial class EnumExtensions
    {"
        );
        foreach (EnumToGenerate enumToGenerate in enumsToGenerate)
        {
            sb.Append
                (
                    @"
                public static string ToStringFast(this "
                )
                .Append(enumToGenerate.Name)
                .Append
                (
                    @" value)
                    => value switch
                    {"
                );
            foreach (string? member in enumToGenerate.Values)
            {
                sb.Append
                    (
                        @"
                "
                    )
                    .Append(enumToGenerate.Name)
                    .Append('.')
                    .Append(member)
                    .Append(" => nameof(")
                    .Append(enumToGenerate.Name)
                    .Append('.')
                    .Append(member)
                    .Append("),");
            }

            sb.Append
            (
                @"
                    _ => value.ToString(),
                };
"
            );
        }

        sb.Append
        (
            @"
    }
}"
        );

        return sb.ToString();
    }

    private readonly struct EnumToGenerate
    {
        public string Name { get; }
        public List<string> Values { get; }

        public EnumToGenerate(string name, List<string> values)
        {
            Name = name;
            Values = values;
        }
    }

    private static class SourceGenerationHelper
    {
        public const string Attribute = @"
namespace NetEscapades.EnumGenerators
{
    [System.AttributeUsage(System.AttributeTargets.Enum)]
    public class EnumExtensionsAttribute : System.Attribute
    {
    }
}";
    }
}
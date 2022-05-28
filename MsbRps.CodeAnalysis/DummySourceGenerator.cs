using System;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace DummySourceGenerator
{
    [Generator]
    public class DummySourceGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context) { }

        public void Execute(GeneratorExecutionContext context)
        {
            context.AddSource("DummyGenerated.cs", SourceText.From(@"class DummyGenerated{}", Encoding.UTF8));
        }
    }
}
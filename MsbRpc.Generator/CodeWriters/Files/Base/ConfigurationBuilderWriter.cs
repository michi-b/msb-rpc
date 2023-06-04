using System.CodeDom.Compiler;
using MsbRpc.Generator.Extensions;
using MsbRpc.Generator.GenerationTree;
using MsbRpc.Generator.Utility;
using static MsbRpc.Generator.Utility.Names;

namespace MsbRpc.Generator.CodeWriters.Files.Base;

internal abstract class ConfigurationBuilderWriter : CodeFileWriter
{
    protected readonly string ClassName;
    protected override string FileName { get; }

    /// <summary>
    ///     fully qualified name of the configuration builder base class
    /// </summary>
    protected abstract string BaseClass { get; }

    protected ConfigurationBuilderWriter(ContractNode contract, string configurationTargetClassName) : base(contract)
    {
        ClassName = GetConfigurationBuilderClassName(configurationTargetClassName);
        FileName = $"{ClassName}{GeneratedFilePostfix}";
    }

    private static string GetConfigurationBuilderClassName(string configurationTargetClassName) => $"{configurationTargetClassName}ConfigurationBuilder";

    protected override void Write(IndentedTextWriter writer)
    {
        writer.WriteLine($"{Contract.AccessibilityKeyword} class {ClassName} : {BaseClass}");
        using (writer.GetBlock(Appendix.None))
        {
            WriteConstructor(writer);
        }
    }

    private void WriteConstructor(IndentedTextWriter writer)
    {
        writer.WriteLine($"public {ClassName}()");
        using (writer.GetBlock())
        {
            WriteConstructorBody(writer);
        }
    }

    protected abstract void WriteConstructorBody(IndentedTextWriter writer);
}
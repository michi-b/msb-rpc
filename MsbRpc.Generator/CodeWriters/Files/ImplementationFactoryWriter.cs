using System.CodeDom.Compiler;
using MsbRpc.Generator.CodeWriters.Files.Base;
using MsbRpc.Generator.Extensions;
using MsbRpc.Generator.GenerationTree;
using MsbRpc.Generator.Utility;
using static MsbRpc.Generator.Utility.Names;

namespace MsbRpc.Generator.CodeWriters.Files;

internal class ImplementationFactoryWriter : CodeFileWriter
{
    private const string ClassNamePostFix = "ImplementationFactory";

    private readonly string _className;
    protected override string FileName { get; }

    public ImplementationFactoryWriter(ContractNode contract) : base(contract)
    {
        _className = $"{contract.PascalCaseName}{ClassNamePostFix}";
        FileName = $"{_className}{GeneratedFilePostfix}";
    }

    protected override void Write(IndentedTextWriter writer)
    {
        writer.WriteLine($"{Contract.AccessibilityKeyword} class {_className} : {Contract.ImplementationFactoryInterfaceName}");
        using (writer.GetBlock(Appendix.None))
        {
            WriteClassBody(writer);
        }
    }

    private void WriteClassBody(IndentedTextWriter writer)
    {
        WriteDelegateField(writer);

        writer.WriteLine();

        WriteConstructor(writer);

        writer.WriteLine();

        WriteCreateMethod(writer);
    }

    private void WriteDelegateField(IndentedTextWriter writer)
    {
        writer.WriteLine($"private {Types.Func}<{Contract.Interface}> {Fields.FactoryCreateFunc};");
    }

    private void WriteConstructor(IndentedTextWriter writer)
    {
        writer.WriteLine($"public {_className}({Types.Func}<{Contract.Interface}> {Parameters.FactoryCreateFunc})");
        using (writer.GetBlock())
        {
            writer.WriteLine($"{Fields.FactoryCreateFunc} = {Parameters.FactoryCreateFunc};");
        }
    }

    private void WriteCreateMethod(IndentedTextWriter writer)
    {
        writer.WriteLine($"public {Contract.Interface} {Methods.ImplementationFactoryCreate}()");
        using (writer.GetBlock())
        {
            writer.WriteLine($"return {Fields.FactoryCreateFunc}();");
        }
    }
}
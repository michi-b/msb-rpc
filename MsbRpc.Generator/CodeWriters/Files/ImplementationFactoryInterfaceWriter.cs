using System.CodeDom.Compiler;
using MsbRpc.Generator.CodeWriters.Files.Base;
using MsbRpc.Generator.Extensions;
using MsbRpc.Generator.GenerationTree;
using MsbRpc.Generator.Utility;
using static MsbRpc.Generator.Utility.Names;

namespace MsbRpc.Generator.CodeWriters.Files;

internal class ImplementationFactoryInterfaceWriter : CodeFileWriter
{
    private readonly string _interfaceName;

    protected override string FileName { get; }

    public ImplementationFactoryInterfaceWriter(ContractNode contract) : base(contract)
    {
        _interfaceName = contract.ImplementationFactoryInterfaceName;
        FileName = $"{_interfaceName}{GeneratedFilePostfix}";
    }

    protected override void Write(IndentedTextWriter writer)
    {
        writer.WriteLine($"{Contract.AccessibilityKeyword} interface {_interfaceName}");
        using (writer.GetBlock(Appendix.None))
        {
            WriteInterfaceBody(writer);
        }
    }

    private void WriteInterfaceBody(IndentedTextWriter writer)
    {
        //todo: add interface parameters for compound contracts
        writer.WriteLine($"{Contract.Interface} {Methods.ImplementationFactoryCreate}();");
    }
}
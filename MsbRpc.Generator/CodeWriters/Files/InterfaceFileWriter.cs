using System.CodeDom.Compiler;
using System.IO;
using MsbRpc.Generator.Extensions;
using MsbRpc.Generator.GenerationTree;

namespace MsbRpc.Generator.CodeWriters.Files;

internal class InterfaceFileWriter : CodeFileWriter
{
    private readonly string _interfaceName;
    private readonly ProcedureCollection _procedures;
    protected override string FileName { get; }

    public InterfaceFileWriter(ContractNode contract, EndPoint endPoint, ProcedureCollection procedures) : base(contract)
    {
        _procedures = procedures;
        _interfaceName = endPoint.Names.ImplementationInterface;
        FileName = $"{_interfaceName}{IndependentNames.GeneratedFilePostfix}";
    }

    protected override void Write(IndentedTextWriter writer)
    {
        writer.WriteLine($"public interface {_interfaceName}");
        using (writer.InBlock(Appendix.None))
        {
            int lastIndex = _procedures.LastIndex;

            for (int i = 0; i < lastIndex; i++)
            {
                WriteInterfaceMethod(writer, _procedures[i]);
            }

            WriteInterfaceMethod(writer, _procedures[lastIndex]);
        }
    }

    private static void WriteInterfaceMethod(TextWriter writer, Procedure procedure)
    {
        writer.Write(procedure.ReturnType.Names.Name);
        writer.Write(' ');
        writer.Write(procedure.Names.Name);
        writer.Write('(');
        {
            ParameterCollection? parameters = procedure.Parameters;
            if (parameters != null)
            {
                for (int i = 0; i < parameters.LastIndex; i++)
                {
                    WriteInterfaceMethodParameter(writer, parameters[i]);
                    writer.Write(", ");
                }

                WriteInterfaceMethodParameter(writer, parameters[parameters.LastIndex]);
            }
        }
        writer.WriteLine(");");
    }

    private static void WriteInterfaceMethodParameter(TextWriter writer, Parameter parameter)
    {
        writer.Write(parameter.Type.Names.Name);
        writer.Write(' ');
        writer.Write(parameter.Names.Name);
    }
}
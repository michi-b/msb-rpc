using System.CodeDom.Compiler;
using MsbRpc.Generator.Extensions;
using MsbRpc.Generator.HelperTree;

namespace MsbRpc.Generator.CodeWriters;

public class InterfaceWriter : CodeWriter
{
    private readonly EndPoint _endPoint;
    private readonly string _interfaceName;
    private readonly ProcedureCollection _procedures;
    protected override string FileName { get; }

    public InterfaceWriter(ContractNode contract, EndPoint endPoint, ProcedureCollection procedures) : base(contract)
    {
        _endPoint = endPoint;
        _procedures = procedures;
        _interfaceName = endPoint.Names.InterfaceType;
        FileName = $"{GeneratedNamespace}.{_interfaceName}{IndependentNames.GeneratedFilePostfix}";
    }

    protected override async ValueTask Write(IndentedTextWriter writer)
    {
        await writer.WriteLineAsync($"public interface {_interfaceName}");
        await writer.EnterBlockAsync();
        {
            int lastIndex = _procedures.LastIndex;

            for (int i = 0; i < lastIndex; i++)
            {
                await WriteInterfaceMethod(writer, _procedures[i]);
                await writer.WriteLineAsync(";");
            }

            await WriteInterfaceMethod(writer, _procedures[lastIndex]);
        }
        await writer.ExitBlockAsync(BlockAdditions.None);
    }

    private static async ValueTask WriteInterfaceMethod(TextWriter writer, Procedure procedure)
    {
        await writer.WriteAsync(procedure.ReturnType.Names.FullName);
        await writer.WriteAsync(' ');
        await writer.WriteAsync(procedure.Names.Name);
        await writer.WriteAsync('(');
        {
            int lastParameterIndex = procedure.LastParameterIndex;
            for (int i = 0; i < lastParameterIndex; i++)
            {
                await WriteInterfaceMethodParameter(writer, procedure.Parameters[i]);
                await writer.WriteAsync(", ");
            }
        }
        await writer.WriteLineAsync(");");
    }

    private static async ValueTask WriteInterfaceMethodParameter(TextWriter writer, Parameter parameter)
    {
        await writer.WriteAsync(parameter.Type.Names.FullName);
        await writer.WriteAsync(' ');
        await writer.WriteAsync(parameter.Names.Name);
    }
}
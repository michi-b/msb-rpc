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

    protected override async ValueTask WriteAsync(IndentedTextWriter writer)
    {
        await writer.WriteLineAsync($"public interface {_interfaceName}");
        await writer.EnterBlockAsync();
        {
            int lastIndex = _procedures.LastIndex;

            for (int i = 0; i < lastIndex; i++)
            {
                await WriteInterfaceMethodAsync(writer, _procedures[i]);
                await writer.WriteLineAsync(";");
            }

            await WriteInterfaceMethodAsync(writer, _procedures[lastIndex]);
        }
        await writer.ExitBlockAsync(BlockAdditions.None);
    }

    private static async ValueTask WriteInterfaceMethodAsync(TextWriter writer, Procedure procedure)
    {
        await writer.WriteAsync(procedure.ReturnType.Names.Name);
        await writer.WriteAsync(' ');
        await writer.WriteAsync(procedure.Names.PascalCaseName);
        await writer.WriteAsync('(');
        {
            if (procedure.TryGetParameters(out ParameterCollection? parameters) && parameters != null)
            {
                for (int i = 0; i < parameters.LastIndex; i++)
                {
                    await WriteInterfaceMethodParameterAsync(writer, parameters[i]);
                    await writer.WriteAsync(", ");
                }
            }
        }
        await writer.WriteLineAsync(");");
    }

    private static async ValueTask WriteInterfaceMethodParameterAsync(TextWriter writer, Parameter parameter)
    {
        await writer.WriteAsync(parameter.Type.Names.Name);
        await writer.WriteAsync(' ');
        await writer.WriteAsync(parameter.Names.Name);
    }
}
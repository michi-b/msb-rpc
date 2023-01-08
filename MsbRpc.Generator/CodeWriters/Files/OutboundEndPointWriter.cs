using System.CodeDom.Compiler;
using MsbRpc.Generator.CodeWriters.Utility;
using MsbRpc.Generator.GenerationTree;
using static MsbRpc.Generator.CodeWriters.Utility.IndependentCode;
using static MsbRpc.Generator.CodeWriters.Utility.IndependentNames;

namespace MsbRpc.Generator.CodeWriters.Files;

internal class OutboundEndPointWriter : EndPointWriter
{
    public OutboundEndPointWriter(EndPointNode endPoint) : base(endPoint) { }

    protected override void WriteClassHeader(IndentedTextWriter writer)
    {
        writer.WriteLine($"public class {Name}");
        writer.Indent++;
        writer.WriteLine($": {Types.OutboundEndPoint}<{Name}, {Procedures.ProcedureEnumName}>");
        writer.Indent--;
    }

    protected override void WriteConstructorsAndFactoryMethods(IndentedTextWriter writer)
    {
        // private constructor accepting a concrete logger
        writer.WriteLine($"private {Name}");
        using (writer.GetParenthesesBlock(Appendix.None))
        {
            writer.WriteLine($"{MessengerParameter},");
            writer.WriteLine($"{GetLoggerParameterLine(Name)},");
            writer.WriteLine($"{InitialBufferSizeParameterLine}");
        }

        writer.WriteLine(" : base");
        using (writer.GetParenthesesBlock(Appendix.None))
        {
            writer.WriteLine($"{Parameters.Messenger},");
            writer.WriteLine($"{Parameters.Logger},");
            writer.WriteLine(Parameters.InitialBufferSize);
        }

        writer.WriteLine(" { }");
        writer.WriteLine();

        // public async factory method
        writer.WriteLine($"public static async {Types.VaLueTask}<{Name}> {Methods.ConnectAsync}");
        using (writer.GetParenthesesBlock())
        {
            writer.WriteLine($"{IPAddressParameter},");
            writer.WriteLine($"{PortParameter},");
            writer.WriteLine($"{LoggerFactoryNullableParameter},");
            writer.WriteLine($"{InitialBufferSizeParameterLine}");
        }

        using (writer.GetBlock())
        {
            writer.WriteLine($"{Types.LoggerInterface}<{Name}> {Variables.Logger} = {GetCreateLoggerArgumentLine(Name)};");

            writer.Write($"{Types.Messenger} {Variables.Messenger} = await {Types.MessengerFactory}.{Methods.ConnectAsync}");
            writer.WriteLine($"(new {Types.IPEndPoint}({Parameters.IPAddress}, {Parameters.Port}), {Variables.Logger});");

            writer.WriteLine($"return new {Name}({Variables.Messenger}, {Variables.Logger}, {Parameters.InitialBufferSize});");
        }
    }

    protected override void WriteProcedures(IndentedTextWriter writer)
    {
        //todo: implement
    }

    protected override void WriteProcedureEnumOverrides(IndentedTextWriter writer)
    {
        base.WriteProcedureEnumOverrides(writer);

        writer.WriteLine();

        writer.Write($"protected override int {Methods.GetProcedureId}({Procedures.ProcedureEnumName} {Parameters.Procedure}) => ");
        writer.WriteLine($"{Procedures.ProcedureEnumExtensionsName}.{Methods.GetProcedureId}({Parameters.Procedure});");
    }
}
using System.CodeDom.Compiler;
using System.IO;
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
        for (int i = 0; i < Procedures.Length; i++)
        {
            if (i > 0)
            {
                writer.WriteLine();
            }

            ProcedureNode procedure = Procedures[i];
            WriteProcedureHeader(writer, procedure);
            using (writer.GetBlock())
            {
                WriteProcedureBody(writer, procedure);
            }
        }
    }

    private static void WriteProcedureHeader(IndentedTextWriter writer, ProcedureNode procedure)
    {
        writer.Write("public async ");
        writer.Write
        (
            procedure.HasReturnValue
                ? $"{Types.VaLueTask}<{procedure.ReturnType.Name}>"
                : $"{Types.VaLueTask}"
        );
        writer.WriteLine($" {procedure.Name}{AsyncPostFix}");
        using (writer.GetParenthesesBlock())
        {
            ParameterCollectionNode? parameters = procedure.Parameters;
            if (parameters != null)
            {
                for (int i = 0; i < parameters.Count; i++)
                {
                    ParameterNode parameter = parameters[i];
                    writer.WriteLine
                    (
                        i < parameters.Count - 1
                            ? $"{parameter.Type.Name} {parameter.Name},"
                            : $"{parameter.Type.Name} {parameter.Name}"
                    );
                }
            }
        }
    }

    private static void WriteProcedureBody(TextWriter writer, ProcedureNode procedure)
    {
        writer.WriteLine($"base.{Methods.AssertIsOperable}();");

        writer.WriteLine();

        ParameterCollectionNode? parameters = procedure.Parameters;

        string getProcedureIdExpression = $"this.{Methods.GetProcedureId}({procedure.ProcedureEnumValue})";

        if (parameters != null)
        {
            foreach (ParameterNode parameter in parameters.ConstantSizeParameters)
            {
                writer.WriteLine($"const int {parameter.SizeVariableName} = {parameter.Type.ConstantSizeExpression};");
            }

            writer.Write($"const int {Variables.ConstantArgumentSizeSum} = ");

            for (int i = 0; i < parameters.ConstantSizeParameters.Count; i++)
            {
                ParameterNode parameter = parameters.ConstantSizeParameters[i];
                if (i > 0)
                {
                    writer.Write("+ ");
                }

                writer.Write(parameter.SizeVariableName);
                writer.WriteLine(i == parameters.ConstantSizeParameters.Count - 1 ? ";" : string.Empty);
            }

            writer.WriteLine();

            writer.WriteLine($"{RequestInitializationWithoutParameters}({getProcedureIdExpression}, {Variables.ConstantArgumentSizeSum});");
            writer.WriteLine($"{RequestWriterInitializationStatement}");

            writer.WriteLine();

            foreach (ParameterNode parameter in parameters)
            {
                writer.WriteLine(parameter.WriteToRequestWriterStatement);
            }
        }
        else
        {
            writer.WriteLine($"{RequestInitializationWithoutParameters}({getProcedureIdExpression});");
        }

        writer.WriteLine();

        if (procedure.HasReturnValue)
        {
            writer.WriteLine(SendRequestStatementWithResponse);
            writer.WriteLine(ResponseReaderInitializationStatement);
            writer.WriteLine(procedure.ReturnType.GetResponseReadStatement());
            writer.WriteLine($"return {Variables.Result};");
        }
        else
        {
            writer.WriteLine(SendRequestStatement);
        }
    }

    protected override void WriteProcedureEnumOverrides(IndentedTextWriter writer)
    {
        base.WriteProcedureEnumOverrides(writer);

        writer.WriteLine();

        writer.Write($"protected override int {Methods.GetProcedureId}({Procedures.ProcedureEnumName} {Parameters.Procedure}) => ");
        writer.WriteLine($"{Procedures.ProcedureEnumExtensionsName}.{Methods.GetProcedureId}({Parameters.Procedure});");
    }
}
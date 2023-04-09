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
        writer.WriteLine($": {Types.OutboundEndPoint}<{Procedures.ProcedureEnumType}>");
        writer.Indent--;
    }

    protected override void WriteConstructorsAndFactoryMethods(IndentedTextWriter writer)
    {
        // private constructor accepting a concrete logger
        writer.WriteLine($"public {Name}");
        using (writer.GetParenthesesBlock(Appendix.None))
        {
            writer.WriteLine($"{MessengerParameter},");
            writer.WriteLine(OutboundEndPointConfigurationParameter);
        }

        writer.WriteLine(" : base");
        using (writer.GetParenthesesBlock(Appendix.None))
        {
            writer.WriteLine($"{Parameters.Messenger},");
            writer.WriteLine($"{Parameters.Configuration}");
        }

        writer.WriteLine(" { }");
        writer.WriteLine();

        // public async factory method
        writer.WriteLine($"public static async {Types.VaLueTask}<{Name}> {Methods.ConnectAsync}");
        using (writer.GetParenthesesBlock())
        {
            writer.WriteLine($"{IPEndPointParameter},");
            writer.WriteLine(OutboundEndPointConfigurationParameter);
        }

        using (writer.GetBlock())
        {
            writer.WriteLine($"{Types.Messenger} {Variables.Messenger} = await {Types.MessengerFactory}.{Methods.ConnectAsync}({Parameters.IPEndPoint});");

            writer.WriteLine($"return new {Name}({Variables.Messenger}, {Parameters.Configuration});");
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
                ? $"{Types.VaLueTask}<{procedure.ReturnType.DeclarationSyntax}>"
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
                            ? $"{parameter.Type.DeclarationSyntax} {parameter.Name},"
                            : $"{parameter.Type.DeclarationSyntax} {parameter.Name}"
                    );
                }
            }
        }
    }

    private void WriteProcedureBody(TextWriter writer, ProcedureNode procedure)
    {
        writer.WriteLine($"base.{Methods.AssertIsOperable}();");

        writer.WriteLine();

        ParameterCollectionNode? parameters = procedure.Parameters;

        string getProcedureIdExpression = $"{Name}.{Methods.GetProcedureId}({procedure.ProcedureEnumValue})";

        if (parameters != null)
        {
            foreach (ParameterNode parameter in parameters)
            {
                parameter.WriteSizeVariableInitialization(writer);
            }

            writer.Write($"int {Variables.ArgumentSizeSum} = ");

            for (int i = 0; i < parameters.Count; i++)
            {
                ParameterNode parameter = parameters[i];
                if (i > 0)
                {
                    writer.Write("+ ");
                }

                writer.Write(parameter.SizeVariableName);
                writer.WriteLine(i == parameters.Count - 1 ? ";" : string.Empty);
            }

            writer.WriteLine();

            writer.WriteLine($"{RequestInitializationWithoutParameters}({getProcedureIdExpression}, {Variables.ArgumentSizeSum});");
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

        writer.Write($"private static int {Methods.GetProcedureId}({Procedures.ProcedureEnumType} {Parameters.Procedure}) => ");
        writer.WriteLine($"{Procedures.ProcedureEnumExtensionsName}.{Methods.GetProcedureId}({Parameters.Procedure});");
    }
}
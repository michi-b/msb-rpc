using System.CodeDom.Compiler;
using MsbRpc.Generator.CodeWriters.Files.Base;
using MsbRpc.Generator.Extensions;
using MsbRpc.Generator.GenerationTree;
using MsbRpc.Generator.Serialization;
using MsbRpc.Generator.Utility;
using static MsbRpc.Generator.Utility.IndependentCode;
using static MsbRpc.Generator.Utility.Names;

namespace MsbRpc.Generator.CodeWriters.Files;

internal class OutboundEndPointWriter : EndPointWriter
{
    public OutboundEndPointWriter(EndPointNode endPoint) : base(endPoint) { }

    protected override void WriteClassHeader(IndentedTextWriter writer)
    {
        writer.WriteLine($"{ContractAccessibilityKeyword} class {Name}");
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

        ISerialization resultSerialization = procedure.ResultSerialization;
        writer.Write
        (
            resultSerialization.IsVoid
                ? $"{Types.VaLueTask}"
                : $"{Types.VaLueTask}<{resultSerialization.DeclarationSyntax}>"
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
                    writer.Write($"{parameter.Serialization.DeclarationSyntax} {parameter.Name}");
                    writer.WriteLine(i < parameters.Count - 1 ? "," : string.Empty);
                }
            }
        }
    }

    private void WriteProcedureBody(IndentedTextWriter writer, ProcedureNode procedure)
    {
        writer.WriteLine($"base.{Methods.AssertIsOperable}();");

        writer.WriteLine();

        ParameterCollectionNode? parameters = procedure.Parameters;

        string getProcedureIdExpression = $"{Name}.{Methods.GetProcedureId}({procedure.ProcedureEnumValue})";

        if (parameters is { IsAnySerializable: true })
        {
            foreach (ParameterNode parameter in parameters)
            {
                string sizeVariableName = parameter.SizeVariableName;
                writer.Write($"int {sizeVariableName} = ");
                parameter.Serialization.WriteSizeExpression(writer, parameter.Name);
                writer.WriteLine(";");
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
                writer.WriteFinalizedSerializationStatement(parameter.Serialization, Variables.RequestWriter, parameter.Name);
            }
        }
        else
        {
            writer.WriteLine($"{RequestInitializationWithoutParameters}({getProcedureIdExpression});");
        }

        writer.WriteLine();

        ISerialization resultSerialization = procedure.ResultSerialization;
        if (resultSerialization.IsVoid)
        {
            writer.WriteLine(SendRequestStatement);
        }
        else
        {
            writer.WriteLine(SendRequestStatementWithResponse);
            writer.WriteLine(ResponseReaderInitializationStatement);

            writer.Write($"{resultSerialization.DeclarationSyntax} {Variables.Result} = ");
            writer.WriteFinalizedDeserializationStatement(resultSerialization, Variables.ResponseReader);

            writer.WriteLine($"return {Variables.Result};");
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
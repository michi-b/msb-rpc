using System.CodeDom.Compiler;
using MsbRpc.Generator.CodeWriters.Utility;
using MsbRpc.Generator.GenerationTree;
using static MsbRpc.Generator.CodeWriters.Utility.IndependentNames;
using static MsbRpc.Generator.CodeWriters.Utility.IndependentCode;

namespace MsbRpc.Generator.CodeWriters.Files;

internal class InboundEndPointWriter : EndPointWriter
{
    public InboundEndPointWriter(EndPointNode endPoint) : base(endPoint) { }

    protected override void WriteClassHeader(IndentedTextWriter writer)
    {
        writer.WriteLine($"public class {Name}");
        writer.Indent++;
        writer.WriteLine($": {Types.InboundEndPoint}<{Name}, {Procedures.ProcedureEnumName}, {Contract.InterfaceName}>");
        writer.Indent--;
    }

    protected override void WriteConstructorsAndFactoryMethods(IndentedTextWriter writer)
    {
        string contractImplementationParameterLine = $"{Contract.InterfaceName} {Parameters.ContractImplementation}";
        string contractImplementationArgumentLine = $"{Parameters.ContractImplementation}";

        // public constructor accepting a logger factory
        writer.WriteLine($"public {Name}");
        using (writer.GetParenthesesBlock(Appendix.None))
        {
            writer.WriteLine($"{MessengerParameter},");
            writer.WriteLine($"{contractImplementationParameterLine},");
            writer.WriteLine($"{NullableLoggerFactoryParameter},");
            writer.WriteLine($"{InitialBufferSizeParameterLine}");
        }

        writer.WriteLine(" : this");
        using (writer.GetParenthesesBlock(Appendix.None))
        {
            writer.WriteLine($"{Parameters.Messenger},");
            writer.WriteLine($"{contractImplementationArgumentLine},");
            writer.WriteLine($"{GetCreateLoggerArgumentLine(Name)},");
            writer.WriteLine($"{Parameters.InitialBufferSize}");
        }

        writer.WriteLine(" { }");

        writer.WriteLine();

        // private constructor accepting a concrete logger
        writer.WriteLine($"private {Name}");
        using (writer.GetParenthesesBlock(Appendix.None))
        {
            writer.WriteLine($"{MessengerParameter},");
            writer.WriteLine($"{contractImplementationParameterLine},");
            writer.WriteLine($"{GetCreateLoggerParameterLine(Name)},");
            writer.WriteLine($"{InitialBufferSizeParameterLine}");
        }

        writer.WriteLine(" : base");
        using (writer.GetParenthesesBlock(Appendix.None))
        {
            writer.WriteLine($"{Parameters.Messenger},");
            writer.WriteLine($"{contractImplementationArgumentLine},");
            writer.WriteLine($"{Parameters.Logger},");
            writer.WriteLine($"{Parameters.InitialBufferSize}");
        }

        writer.WriteLine(" { }");
    }

    protected override void WriteProcedures(IndentedTextWriter writer)
    {
        WriteExecuteMethod(writer);
        foreach (ProcedureNode procedure in Procedures)
        {
            writer.WriteLine();
            WriteProcedure(writer, procedure);
        }
    }

    private void WriteExecuteMethod(IndentedTextWriter writer)
    {
        writer.WriteLine($"protected override {Types.Response} {Methods.InboundEndPointExecute}");
        using (writer.GetParenthesesBlock())
        {
            writer.WriteLine($"{Procedures.ProcedureEnumParameter},");
            writer.WriteLine(RequestParameter);
        }

        string GetSwitchCaseExpression(ProcedureNode procedure)
            => procedure.HasParameters ? $"{procedure.Name}({Parameters.Request})" : $"{procedure.Name}()";

        using (writer.GetBlock())
        {
            writer.WriteProcedureReturnSwitch(Procedures, GetSwitchCaseExpression);
        }
    }

    private static void WriteProcedure(IndentedTextWriter writer, ProcedureNode procedure)
    {
        ParameterCollectionNode? parameters = procedure.Parameters;

        writer.WriteLine(GetInboundProcedureMethodSignature(procedure));
        using (writer.GetBlock())
        {
            //read arguments into local variables
            if (parameters != null)
            {
                writer.WriteLine($"{Types.BufferReader} {Variables.RequestReader} = {Parameters.Request}.{Methods.GetReader}();");
                writer.WriteLine();
                foreach (ParameterNode parameter in parameters)
                {
                    writer.WriteLine($"{parameter.ArgumentVariableName} = {parameter.Type.ReadFromRequestReaderExpression};");
                }

                writer.WriteLine();
            }

            //call the contract implementation
        }
    }

    protected override void WriteProcedureEnumOverrides(IndentedTextWriter writer) { }
}
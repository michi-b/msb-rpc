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
            writer.WriteLine(LocalConfigurationParameter);
        }

        writer.WriteLine(" : base");
        using (writer.GetParenthesesBlock(Appendix.None))
        {
            writer.WriteLine($"{Parameters.Messenger},");
            writer.WriteLine($"{contractImplementationArgumentLine},");
            writer.WriteLine(Parameters.Configuration);
        }

        writer.WriteLine(" { }");
    }

    protected override void WriteConfiguration(IndentedTextWriter writer)
    {
        writer.WriteLine($"public class {Types.LocalConfiguration} : {Types.InboundEndPointConfiguration} {{}}");
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

        string GetSwitchCaseExpression(ProcedureNode procedure) => procedure.HasParameters ? $"{procedure.Name}({Parameters.Request})" : $"{procedure.Name}()";

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
                    writer.WriteLine(parameter.GetRequestReadStatement());
                }

                writer.WriteLine();
            }

            TypeNode returnType = procedure.ReturnType;
            string implementationField = Fields.InboundEndpointImplementation;

            //call the contract implementation
            if (!returnType.IsVoid)
            {
                writer.Write($"{returnType.Name} {Variables.Result} = ");
            }

            writer.Write($"{implementationField}.{procedure.Name}");
            writer.WriteLine(parameters != null ? $"({parameters.GetValueArgumentsString()});" : "();");

            writer.WriteLine();

            //return the result
            if (returnType.IsVoid)
            {
                writer.WriteLine(ReturnEmptyResponseStatement);
            }
            else
            {
                if (returnType.IsConstantSize)
                {
                    writer.WriteLine($"const int {Variables.ResultSize} = {returnType.ConstantSizeExpression};");
                    writer.WriteLine();
                    writer.WriteReturnResultResponse();
                }
            }
        }
    }
}
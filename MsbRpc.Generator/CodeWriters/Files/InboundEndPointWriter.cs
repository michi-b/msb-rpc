using System.CodeDom.Compiler;
using System.Linq;
using MsbRpc.Generator.CodeWriters.Files.Base;
using MsbRpc.Generator.Extensions;
using MsbRpc.Generator.GenerationTree;
using MsbRpc.Generator.Serialization;
using MsbRpc.Generator.Utility;
using static MsbRpc.Generator.Utility.IndependentCode;
using static MsbRpc.Generator.Utility.Names;
using static MsbRpc.Generator.Utility.Names.Types;

namespace MsbRpc.Generator.CodeWriters.Files;

internal class InboundEndPointWriter : EndPointWriter
{
    public InboundEndPointWriter(EndPointNode endPoint) : base(endPoint) { }

    protected override void WriteClassHeader(IndentedTextWriter writer)
    {
        writer.WriteLine($"{Contract.AccessibilityKeyword} class {Name}");
        writer.Indent++;
        writer.WriteLine($": {InboundEndPoint}<{Procedures.ProcedureEnumType}, {Contract.Interface}>");
        writer.Indent--;
    }

    protected override void WriteConstructorsAndFactoryMethods(IndentedTextWriter writer)
    {
        string contractImplementationParameterLine = $"{Contract.Interface} {Parameters.ContractImplementation}";

        // public constructor accepting a logger factory
        writer.WriteLine($"public {Name}");
        using (writer.GetParenthesesBlock(Appendix.None))
        {
            writer.WriteLine($"{MessengerParameter},");
            writer.WriteLine($"{contractImplementationParameterLine},");
            writer.WriteLine($"{InboundEndPointConfigurationParameter}");
        }

        writer.WriteLine(" : base");
        using (writer.GetParenthesesBlock(Appendix.None))
        {
            writer.WriteLine($"{Parameters.Messenger},");
            writer.WriteLine($"{Parameters.ContractImplementation},");
            writer.WriteLine(Parameters.Configuration);
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
        writer.WriteLine($"protected override {Response} {Methods.InboundEndPointExecute}");
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

    private void WriteProcedure(IndentedTextWriter writer, ProcedureNode procedure)
    {
        ParameterCollectionNode? parameters = procedure.Parameters;

        writer.WriteLine(GetInboundProcedureMethodSignature(procedure));
        using (writer.GetBlock())
        {
            //read arguments into local variables
            if (parameters != null)
            {
                bool hasAnySerializableParameters = parameters.Any(p => p.Serialization.IsResolved);

                foreach (ParameterNode parameter in parameters)
                {
                    writer.WriteLine(parameter.GetDeclarationStatement());
                }

                writer.WriteLine();

                if (hasAnySerializableParameters)
                {
                    using (writer.GetTryBlock())
                    {
                        writer.WriteLine($"{BufferReader} {Variables.RequestReader} = {Parameters.Request}.{Methods.GetReader}();");
                        writer.WriteLine();
                        WriteReadParametersFromRequestReaderLines(writer, parameters);
                    }
                }
                else
                {
                    WriteReadParametersFromRequestReaderLines(writer, parameters);
                }

                if (hasAnySerializableParameters)
                {
                    writer.WriteRpcExecutionExceptionCatchBlock(Procedures, procedure, RpcExecutionStageArgumentDeserialization);
                }

                writer.WriteLine();
            }

            ISerialization resultSerialization = procedure.ResultSerialization;

            string allValueArgumentsString = parameters != null ? parameters.GetAllValueArgumentsString() : string.Empty;
            string implementationCallStatement = $"{Fields.InboundEndpointImplementation}.{procedure.Name}({allValueArgumentsString});";

            //call the contract implementation
            if (!resultSerialization.IsVoid)
            {
                writer.WriteLine($"{resultSerialization.DeclarationSyntax} {Variables.Result};");
                writer.WriteLine();
            }

            using (writer.GetTryBlock())
            {
                if (!resultSerialization.IsVoid)
                {
                    writer.Write($"{Variables.Result} = ");
                }

                writer.WriteLine(implementationCallStatement);
            }

            writer.WriteRpcExecutionExceptionCatchBlock(Procedures, procedure, RpcExecutionStageImplementationExecution);

            writer.WriteLine();

            //return the result

            if (!resultSerialization.IsVoid)
            {
                writer.WriteLine($"{Response} {Variables.Response};");

                writer.WriteLine();

                using (writer.GetTryBlock())
                {
                    writer.Write($"int {Variables.ResultSize} = ");
                    resultSerialization.WriteSizeExpression(writer, Variables.Result);
                    writer.WriteLine(";");

                    writer.WriteLine();
                    writer.WriteLine
                    (
                        $"{Variables.Response} = {Fields.EndPointBuffer}.{Methods.GetResponse}("
                        + $"{Fields.InboundEndpointImplementation}.{Properties.RanToCompletion}, "
                        + $"{Variables.ResultSize});"
                    );
                    writer.WriteLine(GetResponseWriterStatement);
                    writer.WriteFinalizedSerializationStatement(resultSerialization, Variables.ResponseWriter, Variables.Result);
                    writer.WriteLine(ReturnResponseStatement);
                }

                writer.WriteRpcExecutionExceptionCatchBlock(Procedures, procedure, RpcExecutionStageResponseSerialization);
            }
            else
            {
                writer.WriteLine(ReturnEmptyResponseStatement);
            }
        }
    }

    private static void WriteReadParametersFromRequestReaderLines(IndentedTextWriter writer, ParameterCollectionNode parameters)
    {
        foreach (ParameterNode parameter in parameters)
        {
            writer.Write($"{parameter.ArgumentVariableName} = ");
            writer.WriteFinalizedDeserializationStatement(parameter.Serialization, Variables.RequestReader);
        }
    }
}
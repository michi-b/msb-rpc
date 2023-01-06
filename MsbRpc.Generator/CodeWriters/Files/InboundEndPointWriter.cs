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
        writer.Write($"public class {Name} : {Types.InboundEndPoint}");
        writer.WriteLine($"<{Name}, {Procedures.EnumName}, {Contract.InterfaceName}>");
    }

    protected override void WriteConstructors(IndentedTextWriter writer)
    {
        string contractImplementationParameterLine = $"{Contract.InterfaceName} {Parameters.ContractImplementation}";
        string contractImplementationArgumentLine = $"{Parameters.ContractImplementation}";

        // public constructor accepting a logger factory
        writer.WriteLine($"public {Name}");
        using (writer.GetParenthesesBlock(Appendix.None))
        {
            writer.WriteLine($"{MessengerParameterLine},");
            writer.WriteLine($"{contractImplementationParameterLine},");
            writer.WriteLine($"{NullableLoggerFactoryParameterLine},");
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
            writer.WriteLine($"{MessengerParameterLine},");
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

    protected override void WriteProcedures(IndentedTextWriter writer) { }

    protected override void WriteProcedureEnumOverrides(IndentedTextWriter writer) { }
}
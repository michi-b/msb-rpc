using System.CodeDom.Compiler;
using static MsbRpc.Generator.GenerationHelpers.GeneralNames;

namespace MsbRpc.Generator.GenerationHelpers;

public static class GeneralCode
{
    public const string MessengerParameterLine = $"{MessengerType} {MessengerParameter},";
    public const string LoggerFactoryInterfaceParameter = $"{LoggerFactoryInterfaceType} {LoggerFactoryParameter},";

    public static void WriteLoggerArgumentFromFactoryParameterLines(this IndentedTextWriter writer, string categoryTypeName)
    {
        writer.WriteLine($"{LoggerFactoryParameter} != null");
        writer.Indent++;
        writer.WriteLine($"? {CreateLoggerMethod}.CreateLogger<{categoryTypeName}>({LoggerFactoryParameter})");
        writer.WriteLine($": new {NullLoggerType}<{categoryTypeName}>(),");
        writer.Indent--;
    }
}
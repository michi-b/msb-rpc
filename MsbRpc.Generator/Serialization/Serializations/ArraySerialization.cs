using System.CodeDom.Compiler;
using MsbRpc.Generator.Extensions;
using MsbRpc.Generator.Utility;
using static MsbRpc.Generator.Utility.IndependentNames;

namespace MsbRpc.Generator.Serialization.Serializations;

public class ArraySerialization : ISerialization
{
    private const string BufferWriterArgumentName = "writer";
    private const string BufferReaderArgumentName = "reader";
    private const string ElementArgumentName = "element";
    private readonly ISerialization _elementSerialization;
    private readonly bool _needsCastOnDeserialization;
    private readonly int _rank;
    private readonly string _serializerName;

    public bool IsVoid => false;
    public bool IsResolved => _elementSerialization.IsResolved;
    public bool IsConstantSize => false;
    public string DeclarationSyntax => $"{_elementSerialization.DeclarationSyntax}[{new string(',', _rank - 1)}]";
    public bool NeedsSemicolonAfterSerializationStatement => true;

    public ArraySerialization(ISerialization elementSerialization, int rank)
    {
        _elementSerialization = elementSerialization;
        _rank = rank;
        _serializerName = $"{GetSerializerClassReferenceWithoutTypeArgument(rank)}<{_elementSerialization.DeclarationSyntax}>";

        //rank > 9 uses any rank array serializer, which returns an untyped array that needs to be cast to the target type on deserialization
        _needsCastOnDeserialization = rank > 9;
    }

    public void WriteSizeExpression(IndentedTextWriter writer, string targetExpression)
    {
        writer.Write($"{_serializerName}.{Methods.SerializerGetSize}");
        using (writer.GetParenthesesBlock(Appendix.None))
        {
            writer.WriteLine($"{targetExpression},");
            if (!_elementSerialization.IsConstantSize)
            {
                writer.Write($"({ElementArgumentName}) => ");
            }

            //value argument is not used if inner value is constant size
            writer.WriteSerializationSizeExpression(_elementSerialization, ElementArgumentName);
            writer.WriteLine();
        }
    }

    public void WriteSerializationStatement(IndentedTextWriter writer, string bufferWriterExpression, string valueExpression)
    {
        writer.WriteLine($"{_serializerName}.{Methods.SerializerWrite}");
        using (writer.GetParenthesesBlock(Appendix.None))
        {
            writer.WriteLine($"ref {bufferWriterExpression},");
            writer.WriteLine($"{valueExpression},");

            writer.WriteLine($"(ref {Types.BufferWriter} {BufferWriterArgumentName}, {_elementSerialization.DeclarationSyntax} {ElementArgumentName}) => ");
            using (writer.GetBlock())
            {
                writer.WriteFinalizedSerializationStatement(_elementSerialization, BufferWriterArgumentName, ElementArgumentName);
            }
        }
    }

    public void WriteDeserializationExpression(IndentedTextWriter writer, string bufferReaderExpression)
    {
        if (_needsCastOnDeserialization)
        {
            writer.Write($"({DeclarationSyntax}) ");
        }

        writer.WriteLine($"{_serializerName}.{Methods.SerializerRead}");
        using (writer.GetParenthesesBlock(Appendix.None))
        {
            writer.WriteLine($"ref {bufferReaderExpression},");
            writer.Write($"(ref {Types.BufferReader} {BufferReaderArgumentName}) => ");
            writer.WriteDeserializationExpression(_elementSerialization, BufferReaderArgumentName);
            writer.WriteLine();
        }
    }

    private static string GetSerializerClassReferenceWithoutTypeArgument(int rank)
        => rank switch
        {
            1 => Types.ArraySerializer,
            2 => Types.Array2DSerializer,
            3 => Types.Array3DSerializer,
            4 => Types.Array4DSerializer,
            5 => Types.Array5DSerializer,
            6 => Types.Array6DSerializer,
            7 => Types.Array7DSerializer,
            8 => Types.Array8DSerializer,
            9 => Types.Array9DSerializer,
            _ => Types.AnyRankArraySerializer
        };
}
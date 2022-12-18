using System.CodeDom.Compiler;
using System.Diagnostics;
using MsbRpc.Generator.Extensions;
using MsbRpc.Generator.GenerationHelpers.Code;
using MsbRpc.Generator.GenerationHelpers.Extensions;
using MsbRpc.Generator.GenerationHelpers.Names;
using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.GenerationHelpers;

public readonly struct ProcedureGenerator
{
    private static class VariableNames
    {
        public const string RequestWriter = "writer";
        public const string ConstantArgumentsSizeSum = "constantSerializedSize";
    }

    private readonly string _fullName;
    private readonly string _name;
    private readonly ParameterGenerator[] _parameters;
    private readonly bool _hasParameters;
    private readonly string _parametersString;
    private readonly TypeGenerator _returnType;
    private readonly bool _invertsDirection;
    private readonly string _constantArgumentsSizeSumCodeLine;

    public ProcedureGenerator(ProcedureInfo info, string procedureEnumName)
    {
        _name = info.Name;

        _parameters = new ParameterGenerator[info.Parameters.Length];

        List<string> constantSerializationSumParts = new(info.Parameters.Length);

        for (int i = 0; i < info.Parameters.Length; i++)
        {
            ParameterInfo parameterInfo = info.Parameters[i];

            var parameter = new ParameterGenerator(parameterInfo);

            if (parameter.IsConstantSize)
            {
                constantSerializationSumParts.Add(parameter.ConstantSizeVariableName);
            }

            _parameters[i] = parameter;
        }

        _constantArgumentsSizeSumCodeLine =
            $"const int {VariableNames.ConstantArgumentsSizeSum} = {string.Join(" + ", constantSerializationSumParts)};";

        _hasParameters = _parameters.Length > 0;

        _parametersString = _parameters.GetString();

        _invertsDirection = info.InvertsDirection;

        _returnType = new TypeGenerator(info.ReturnType);

        _fullName = procedureEnumName + '.' + _name;
    }

    public void GenerateInterfaceMethod(IndentedTextWriter writer)
    {
        writer.Write($"{_returnType.Name} {_name}");

        using (writer.EncloseInParentheses())
        {
            if (_hasParameters)
            {
                writer.Write(_parametersString);
            }
        }

        writer.WriteLineSemicolon();
    }

    public void GenerateEnumField(IndentedTextWriter writer, int value, bool addCommaAtEndOfLine)
    {
        writer.Write(_name);
        writer.Write(" = ");
        writer.Write(value);

        if (addCommaAtEndOfLine)
        {
            writer.WriteCommaDelimiter();
        }

        writer.WriteLine();
    }

    public void GenerateEnumToNameCase(IndentedTextWriter writer)
    {
        writer.WriteLine("{0} => nameof({0}),", _fullName);
    }

    public void GenerateGetInvertsDirectionCase(IndentedTextWriter writer)
    {
        writer.WriteLine($"{_fullName} => {(_invertsDirection ? "true" : "false")},");
    }

    public void GenerateRequestMethod(IndentedTextWriter writer)
    {
        Debug.Assert(_returnType.SerializationKind.GetIsPrimitive(), "only primitives are implemented right now");

        //header
        writer.Write($"{GeneralNames.Types.VaLueTask}<{_returnType.Name}> {_name}{GeneralNames.AsyncSuffix}(");
        if (_hasParameters)
        {
            writer.Write($"{_parametersString}, ");
        }

        writer.WriteLine($"{GeneralCode.CancellationTokenParameter})");

        //body
        using (writer.EncloseInBlock())
        {
            writer.WriteLine($"{EndPointNames.Methods.EnterCalling}();");

            if (_hasParameters)
            {
                writer.WriteLine();
                foreach (ParameterGenerator parameter in _parameters)
                {
                    writer.WriteLine(parameter.ConstantSizeVariableInitializationLine);
                }

                writer.WriteLine();
                writer.WriteLine(_constantArgumentsSizeSumCodeLine);
            }

            writer.WriteLine();
            writer.WriteLine
            (
                EndPointCode.GetRequestWriterCodeLine
                (
                    VariableNames.RequestWriter,
                    _hasParameters ? VariableNames.ConstantArgumentsSizeSum : "0"
                )
            );
        }
    }
}
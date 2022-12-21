using System.CodeDom.Compiler;
using System.Diagnostics;
using MsbRpc.Generator.Extensions;
using MsbRpc.Generator.GenerationHelpers.Code;
using MsbRpc.Generator.GenerationHelpers.Extensions;
using MsbRpc.Generator.GenerationHelpers.Names;
using MsbRpc.Generator.Info;
using static MsbRpc.Generator.GenerationHelpers.Names.ProcedureNames;

namespace MsbRpc.Generator.GenerationHelpers;

public readonly struct ProcedureGenerator
{
    private readonly string _fullName;
    private readonly string _name;
    private readonly ParameterGenerator[] _parameters;
    private readonly bool _hasParameters;
    private readonly string _parametersString;
    private readonly TypeGenerator _returnType;
    private readonly bool _invertsDirection;
    private readonly string _constantArgumentsSizeSumCodeLine;
    private readonly string _procedureEnumName;

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
            $"const int {Variables.ConstantArgumentsSizeSum} = {string.Join(" + ", constantSerializationSumParts)};";

        _hasParameters = _parameters.Length > 0;

        _parametersString = _parameters.GetString();

        _invertsDirection = info.InvertsDirection;

        _returnType = new TypeGenerator(info.ReturnType);

        _procedureEnumName = procedureEnumName;

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
        writer.Write($"public async {GeneralNames.Types.VaLueTask}<{_returnType.Name}> {_name}{GeneralNames.AsyncSuffix}(");
        if (_hasParameters)
        {
            writer.Write($"{_parametersString}, ");
        }

        writer.WriteLine($"{GeneralCode.CancellationTokenParameter})");

        //body
        using (writer.EncloseInBlock())
        {
            //enter calling
            writer.WriteLine($"{EndPointNames.Methods.EnterCalling}();");

            //get size for request writer
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

            //get request writer
            writer.WriteLine();
            writer.WriteLine
            (
                EndPointCode.GetRequestWriterCodeLine
                (
                    Variables.RequestWriter,
                    _hasParameters ? Variables.ConstantArgumentsSizeSum : "0"
                )
            );

            //write parameters
            writer.WriteLine();
            foreach (ParameterGenerator parameter in _parameters)
            {
                writer.WriteLine($"{Variables.RequestWriter}.{Methods.BufferWriterWrite}({parameter.Name});");
            }

            //procedure id
            writer.WriteLine();
            writer.WriteLine($"const {_procedureEnumName} {Variables.Procedure} = {_fullName};");

            //send request
            writer.WriteLine();
            writer.Write($"{Types.BufferReader} {Variables.ResponseReader} = await {Methods.SendRequest}(");
            writer.Write($"{Variables.Procedure}, ");
            writer.Write($"{Variables.RequestWriter}.{Properties.BufferWriterBufferProperty}, ");
            writer.WriteLine($"{GeneralNames.Parameters.CancellationToken});");

            //read response
            writer.WriteLine();
            writer.Write($"{_returnType.Name} {Variables.Response} = ");
            writer.WriteLine($"{_returnType.GetBufferReadExpression(Variables.ResponseReader)};");

            //exit calling
            writer.WriteLine();
            writer.WriteLine($"{EndPointNames.Methods.ExitCalling}({Variables.Procedure});");

            //return response
            writer.WriteLine();
            writer.WriteLine($"return {Variables.Response};");
        }
    }
}
using System.CodeDom.Compiler;
using MsbRpc.Generator.Extensions;
using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.Generators;

public class ProcedureGenerator
{
    private readonly string _name;
    private readonly List<ParameterInfo> _parameters;
    private readonly TypeInfo _returnType;

    public ProcedureGenerator(ProcedureInfo info)
    {
        _name = info.Name;

        _parameters = new List<ParameterInfo>(info.Parameters.Length);
        foreach (ParameterInfo parameterInfo in info.Parameters)
        {
            _parameters.Add(parameterInfo);
        }

        _returnType = info.ReturnType;
    }

    public void GenerateInterface(IndentedTextWriter writer)
    {
        writer.Write($"{_returnType.FullName} {_name}");

        using (writer.EncloseInParentheses())
        {
            if (_parameters.Count > 0)
            {
                _parameters[0].GenerateInterface(writer);
                for (int i = 1; i < _parameters.Count; i++)
                {
                    writer.WriteCommaDelimiter();
                    _parameters[i].GenerateInterface(writer);
                }
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

    public void GenerateEnumToNameSwitchLine(IndentedTextWriter writer, string serverProcedureEnumName)
    {
        writer.WriteLine("{0} => nameof({0}),", serverProcedureEnumName + "." + _name);
    }
}
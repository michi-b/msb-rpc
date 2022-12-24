using System.CodeDom.Compiler;
using MsbRpc.Generator.Extensions;
using MsbRpc.Generator.GenerationHelpers.ReusedNames;
using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.GenerationHelpers;

public readonly struct ContractGenerator
{
    public ContractNames Names { get; }

    private readonly EndPointGenerator _clientGenerator;
    private readonly EndPointGenerator _serverGenerator;

    public EndPointGenerator this[EndPointTypeId endPointType]
        => endPointType switch
        {
            EndPointTypeId.Client => _clientGenerator,
            EndPointTypeId.Server => _serverGenerator,
            _ => throw new ArgumentOutOfRangeException(nameof(endPointType), endPointType, null)
        };

    public ContractGenerator(ref ContractInfo info)
    {
        Names = new ContractNames(info.Namespace, info.InterfaceName);

        _clientGenerator = new EndPointGenerator(ref info, Names, EndPointTypeId.Client);
        _serverGenerator = new EndPointGenerator(ref info, Names, EndPointTypeId.Server);
        _clientGenerator.Remote = _serverGenerator;
        _serverGenerator.Remote = _clientGenerator;
    }

    public string GenerateInterface(EndPointTypeId endPoint)
    {
        using IndentedTextWriter writer = CreateCodeWriter();
        this[endPoint].GenerateInterface(writer);
        return writer.GetResult();
    }

    public string GenerateProcedureEnum(EndPointTypeId endPoint)
    {
        using IndentedTextWriter writer = CreateCodeWriter();
        this[endPoint].GenerateProcedureEnum(writer);
        return writer.GetResult();
    }

    public string GenerateProcedureEnumExtensions(EndPointTypeId endPoint)
    {
        using IndentedTextWriter writer = CreateCodeWriter();
        this[endPoint].GenerateProcedureEnumExtensions(writer);
        return writer.GetResult();
    }

    public string GenerateEndPoint(EndPointTypeId endPoint)
    {
        using IndentedTextWriter writer = CreateCodeWriter();
        this[endPoint].GenerateEndPoint(writer);
        return writer.GetResult();
    }

    private IndentedTextWriter CreateCodeWriter()
    {
        IndentedTextWriter writer = new(new StringWriter());

        writer.WriteFileHeader(Names.GeneratedNamespace);

        return writer;
    }
}
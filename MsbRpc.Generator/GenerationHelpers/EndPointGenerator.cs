using System.CodeDom.Compiler;
using System.Collections.Immutable;
using MsbRpc.EndPoints;
using MsbRpc.Generator.Extensions;
using MsbRpc.Generator.GenerationHelpers.Code;
using MsbRpc.Generator.GenerationHelpers.Extensions;
using MsbRpc.Generator.GenerationHelpers.ReusedNames;
using MsbRpc.Generator.Info;
using static MsbRpc.Generator.GenerationHelpers.ReusedNames.EndPointNames;

namespace MsbRpc.Generator.GenerationHelpers;

public class EndPointGenerator
{
    private readonly ProcedureGenerator[] _inboundProcedures;
    private readonly EndPointDirection _initialDirection;
    public readonly bool HasInboundProcedures;
    public readonly EndPointNames Names;

    public EndPointGenerator? Remote { set; private get; }

    public EndPointGenerator(ref ContractInfo contractInfo, ContractNames contractNames, EndPointTypeId endPointType)
    {
        Names = new EndPointNames(ref contractInfo, contractNames, endPointType);
        _initialDirection = endPointType.GetInitialDirection();

        EndPointInfo info = contractInfo[endPointType];

        ImmutableArray<ProcedureInfo> inboundProcedureInfos = info.Procedures;

        HasInboundProcedures = info.HasInboundProcedures;

        if (HasInboundProcedures)
        {
            int inboundProceduresCount = inboundProcedureInfos.Length;
            _inboundProcedures = new ProcedureGenerator[inboundProceduresCount];
            for (int i = 0; i < inboundProceduresCount; i++)
            {
                ProcedureInfo inboundProcedureInfo = inboundProcedureInfos[i];
                _inboundProcedures[i] = new ProcedureGenerator(ref contractInfo, contractNames, ref info, Names, ref inboundProcedureInfo);
            }
        }
        else
        {
            _inboundProcedures = Array.Empty<ProcedureGenerator>();
        }
    }

    public void GenerateInterface(IndentedTextWriter writer)
    {
        writer.WriteLine("public interface {0}", Names.InterfaceType);

        using (writer.EncloseInBlockAsync(BlockOptions.None))
        {
            foreach (ProcedureGenerator procedure in _inboundProcedures)
            {
                procedure.GenerateInterfaceMethod(writer);
            }
        }
    }

    public void GenerateProcedureEnum(IndentedTextWriter writer)
    {
        writer.WriteLine("public enum {0}", Names.InboundProcedureEnumType);

        using (writer.EncloseInBlockAsync(BlockOptions.None))
        {
            int proceduresCount = _inboundProcedures.Length;
            int lastIndex = proceduresCount - 1;
            for (int i = 0; i < proceduresCount; i++)
            {
                _inboundProcedures[i].GenerateEnumField(writer, i, i < lastIndex);
            }
        }
    }

    public void GenerateProcedureEnumExtensions(IndentedTextWriter writer)
    {
        writer.WriteLine("public static class {0}", Names.InboundProcedureEnumExtensionsType);

        const string procedureParameterName = "procedure";

        using (writer.EncloseInBlockAsync(BlockOptions.None))
        {
            GenerateProcedureEnumGetNameExtension(writer, procedureParameterName);
            writer.WriteLine();
            GenerateProcedureEnumGetInvertsDirectionExtension(writer, procedureParameterName);
        }
    }

    public void GenerateEndPoint(IndentedTextWriter writer)
    {
        EndPointGenerator remote = GetRemote();

        writer.Write($"public class {Names.EndPointType} : {IndependentNames.Types.EndPointBaseType}");
        writer.WriteLine($"<{Names.InboundProcedureEnumType}, {remote.Names.InboundProcedureEnumType}>");

        using (writer.EncloseInBlockAsync(BlockOptions.None))
        {
            if (HasInboundProcedures)
            {
                writer.WriteLine($"private readonly {Names.InterfaceType} {Names.InterfaceField};\n");
            }

            GenerateEndpointConstructor(writer);

            foreach (ProcedureGenerator outBoundProcedure in remote._inboundProcedures)
            {
                writer.WriteLine();
                outBoundProcedure.GenerateRequestMethod(writer);
            }

            if (HasInboundProcedures)
            {
                writer.WriteLine();
                GenerateHandleRequest(writer);
            }
        }
    }

    private EndPointGenerator GetRemote()
    {
        if (Remote == null)
        {
            throw new NullReferenceException("remote has not been set, make sure to not call generation code before the remote has been set");
        }

        return Remote;
    }

    private void GenerateHandleRequest(IndentedTextWriter writer)
    {
        string procedureParameterName = Names.InboundProcedureEnumParameter;

        //header
        writer.WriteLine($"protected override {IndependentNames.Types.BufferWriter} HandleRequest");
        using (writer.EncloseInParenthesesBlockAsync())
        {
            writer.WriteLine($"{Names.InboundProcedureEnumType} {procedureParameterName},");
            writer.WriteLine($"{IndependentNames.Types.BufferReader} {Parameters.ArgumentsBufferReader}");
        }

        //body
        using (writer.EncloseInBlockAsync(BlockOptions.None))
        {
            writer.WriteLine($"return {procedureParameterName} switch");
        }
    }

    private void GenerateProcedureEnumGetNameExtension(IndentedTextWriter writer, string procedureParameterName)
    {
        writer.WriteLine("public static string GetProcedureName(this {0} {1})", Names.InboundProcedureEnumType, procedureParameterName);
        using (writer.EncloseInBlockAsync())
        {
            writer.WriteLine("return procedure switch");
            using (writer.EncloseInBlockAsync(BlockOptions.WithTrailingSemicolonAndNewline))
            {
                foreach (ProcedureGenerator procedure in _inboundProcedures)
                {
                    procedure.GenerateEnumToNameCase(writer);
                }

                GenerateProcedureOutOfRangeCase(writer, procedureParameterName);
            }
        }
    }

    private void GenerateProcedureEnumGetInvertsDirectionExtension(IndentedTextWriter writer, string procedureParameterName)
    {
        writer.WriteLine("public static bool GetInvertsDirection(this {0} {1})", Names.InboundProcedureEnumType, procedureParameterName);

        using (writer.EncloseInBlockAsync())
        {
            writer.WriteLine("return {0} switch", procedureParameterName);

            using (writer.EncloseInBlockAsync(BlockOptions.WithTrailingSemicolonAndNewline))
            {
                foreach (ProcedureGenerator procedure in _inboundProcedures)
                {
                    procedure.GenerateGetInvertsDirectionCase(writer);
                }

                GenerateProcedureOutOfRangeCase(writer, procedureParameterName);
            }
        }
    }

    private void GenerateEndpointConstructor(IndentedTextWriter writer)
    {
        //header
        writer.WriteLine($"public {Names.EndPointType}");
        using (writer.EncloseInParenthesesBlockAsync())
        {
            writer.WriteLine(GeneralCode.MessengerParameterLine);
            if (HasInboundProcedures)
            {
                writer.WriteLine($"{Names.InterfaceType} {Names.InterfaceParameter},");
            }

            writer.WriteLine(GeneralCode.LoggerFactoryInterfaceParameter);
            writer.WriteLine(EndPointCode.BufferSizeParameterWithDefaultLine);
        }

        //base constructor invocation
        writer.Indent++;
        {
            writer.WriteLine(": base");
            using (writer.EncloseInParenthesesBlockAsync(false))
            {
                writer.WriteLine($"{IndependentNames.Parameters.Messenger},");
                writer.WriteLine(EndPointCode.GetInitialDirectionArgumentLine(_initialDirection));
                GenerateLoggerArgumentCreation(writer, Names.EndPointType);
                writer.WriteLine(Parameters.BufferSize);
            }

            //body
            writer.WriteLine
            (
                HasInboundProcedures
                    ? $" => {Names.InterfaceField} = {Names.InterfaceParameter};"
                    : " { }"
            );
        }
        writer.Indent--;
    }

    private static void GenerateLoggerArgumentCreation(IndentedTextWriter writer, string categoryTypeName)
    {
        writer.WriteLine($"{IndependentNames.Parameters.LoggerFactory} != null");
        writer.Indent++;
        writer.WriteLine($"? {IndependentNames.Methods.CreateLogger}<{categoryTypeName}>({IndependentNames.Parameters.LoggerFactory})");
        writer.WriteLine($": new {IndependentNames.Types.NullLogger}<{categoryTypeName}>(),");
        writer.Indent--;
    }

    private static void GenerateProcedureOutOfRangeCase(TextWriter writer, string procedureParameterName)
    {
        writer.WriteLine
            ($"_ => throw new {IndependentNames.Types.ArgumentOutOfRangeException}(nameof({procedureParameterName}), {procedureParameterName}, null)");
    }
}
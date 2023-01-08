using System;
using System.CodeDom.Compiler;
using MsbRpc.Generator.CodeWriters.Utility;
using MsbRpc.Generator.Enums;
using MsbRpc.Generator.GenerationTree;
using static MsbRpc.Generator.CodeWriters.Utility.IndependentNames;

namespace MsbRpc.Generator.CodeWriters.Files;

internal abstract class EndPointWriter : CodeFileWriter
{
    protected readonly ContractNode Contract;

    protected readonly EndPointNode EndPoint;

    /// <summary>
    ///     the concrete endpoint class name
    /// </summary>
    protected readonly string Name;

    protected readonly ProcedureCollectionNode Procedures;

    protected override string FileName { get; }

    protected EndPointWriter(EndPointNode endPoint) : base(endPoint.Contract)
    {
        EndPoint = endPoint;
        Contract = EndPoint.Contract;
        Procedures = Contract.Procedures;
        Name = EndPoint.EndPointName;
        FileName = $"{EndPoint.EndPointName}{GeneratedFilePostfix}";
    }

    public static EndPointWriter Get(EndPointNode endPoint)
    {
        return endPoint.Direction switch
        {
            EndPointDirection.Inbound => new InboundEndPointWriter(endPoint),
            EndPointDirection.Outbound => new OutboundEndPointWriter(endPoint),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    protected override void Write(IndentedTextWriter writer)
    {
        WriteClassHeader(writer);

        using (writer.GetBlock(Appendix.None))
        {
            WriteConstructorsAndFactoryMethods(writer);

            writer.WriteLine();

            WriteProcedures(writer);

            writer.WriteLine();

            WriteProcedureEnumOverrides(writer);
        }
    }

    protected virtual void WriteProcedureEnumOverrides(IndentedTextWriter writer)
    {
        WriteGetProcedureNameOverride(writer);

        writer.WriteLine();

        WriteGetProcedureOverride(writer);
    }

    private void WriteGetProcedureNameOverride(IndentedTextWriter writer)
    {
        writer.Write($"protected override {Procedures.ProcedureEnumName} {Methods.GetProcedure}(int {Parameters.ProcedureId}) => ");
        writer.WriteLine($"{Procedures.ProcedureEnumExtensionsName}.{Methods.FromIdProcedureExtension}({Parameters.ProcedureId});");
    }

    private void WriteGetProcedureOverride(IndentedTextWriter writer)
    {
        writer.Write($"protected override string {Methods.GetProcedureName}({Procedures.ProcedureEnumName} {Parameters.Procedure}) => ");
        writer.WriteLine($"{Procedures.ProcedureEnumExtensionsName}.{Methods.GetNameProcedureExtension}({Parameters.Procedure});");
    }

    protected abstract void WriteClassHeader(IndentedTextWriter writer);

    protected abstract void WriteConstructorsAndFactoryMethods(IndentedTextWriter writer);

    protected abstract void WriteProcedures(IndentedTextWriter writer);
}
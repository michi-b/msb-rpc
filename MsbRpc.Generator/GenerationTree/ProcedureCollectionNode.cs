using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using MsbRpc.Generator.Info;
using static MsbRpc.Generator.CodeWriters.Utility.IndependentNames;

namespace MsbRpc.Generator.GenerationTree;

internal class ProcedureCollectionNode : IReadOnlyList<ProcedureNode>
{
    private readonly ProcedureNode[] _procedures;
    public readonly ContractNode Contract;
    public readonly bool IsValid = true;
    public readonly int LastIndex;
    public readonly string ProcedureEnumExtensionsName;
    public readonly string ProcedureEnumName;
    public readonly string ProcedureEnumParameter;

    public ProcedureNode this[int index] => _procedures[index];

    public int Length { get; }
    public int Count => Length;

    public ProcedureCollectionNode
    (
        ImmutableArray<ProcedureInfo> procedures,
        ContractNode contract,
        TypeNodeCache typeNodeCache,
        SourceProductionContext context
    )
    {
        Contract = contract;
        ProcedureEnumName = $"{contract.PascalCaseName}{ProcedurePostfix}";
        ProcedureEnumExtensionsName = $"{ProcedureEnumName}{ExtensionsPostFix}";
        ProcedureEnumParameter = $"{ProcedureEnumName} {Parameters.Procedure}";
        Length = procedures.Length;
        LastIndex = Length - 1;

        _procedures = new ProcedureNode[Length];
        for (int i = 0; i < Length; i++)
        {
            var procedure = new ProcedureNode(procedures[i], this, i, typeNodeCache, context);
            IsValid = IsValid && procedure.IsValid;
            _procedures[i] = procedure;
        }

        IsValid = IsValid && _procedures.Length > 0;
    }

    public IEnumerator<ProcedureNode> GetEnumerator() => ((IEnumerable<ProcedureNode>)_procedures).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using MsbRpc.Generator.Attributes;
using MsbRpc.Generator.Enums;
using MsbRpc.Generator.Extensions;

namespace MsbRpc.Generator.Info;

internal static class ContractInfoParser
{
    // ReSharper disable once SuggestBaseTypeForParameterInConstructor

    public static ContractInfo? Parse(INamedTypeSymbol symbol)
    {
        #region get attributes

        AttributeData? contractAttribute = null;

        // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
        // foreach is more readable to me here
        foreach (AttributeData attribute in symbol.GetAttributes())
        {
            INamedTypeSymbol? attributeClass = attribute.AttributeClass;
            if (attributeClass != null && attributeClass.GetIsRpcContractAttribute())
            {
                contractAttribute = attribute;
            }
        }

        #endregion

        #region parse contract attribute

        if (contractAttribute == null)
        {
            return null;
        }

        RpcContractType? contractType = null;
        foreach (KeyValuePair<string, TypedConstant> argument in contractAttribute.GetArguments())
        {
            // ReSharper disable once ConvertSwitchStatementToSwitchExpression
            // there will probably be more arguments in the future
            switch (argument.Key)
            {
                case "contractType":
                    contractType = (RpcContractType)argument.Value.Value!;
                    break;
            }
        }

        if (contractType == null)
        {
            return null;
        }

        #endregion

        string interfaceName = symbol.Name;
        string namespaceName = symbol.ContainingNamespace.ToDisplayString();
        ImmutableArray<ProcedureInfo> procedures = symbol.GetMembers()
            .Select(m => m as IMethodSymbol)
            .Where(m => m != null)
            .Select(m => new ProcedureInfo(m!))
            .ToImmutableArray();

        if (!symbol.DeclaredAccessibility.TryGet(out ContractAccessibility accessibility))
        {
            return null;
        }
        
        if (procedures.Length == 0)
        {
            return null;
        }

        return new ContractInfo(interfaceName, namespaceName, procedures, contractType.Value, accessibility);
    }
}
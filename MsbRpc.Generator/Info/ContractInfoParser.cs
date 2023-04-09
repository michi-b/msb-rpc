﻿using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using MsbRpc.Generator.Attributes;
using MsbRpc.Generator.Extensions;
using MsbRpc.Generator.Utility;

namespace MsbRpc.Generator.Info;

internal static class ContractInfoParser
{
    // ReSharper disable once SuggestBaseTypeForParameterInConstructor

    public static ContractInfo? Parse(INamedTypeSymbol contract)
    {
        #region get attributes

        AttributeData? contractAttribute = null;

        // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
        // foreach is more readable to me here
        foreach (AttributeData attribute in contract.GetAttributes())
        {
            INamedTypeSymbol? attributeClass = attribute.AttributeClass;
            if (attributeClass != null && attributeClass.IsRpcContractAttribute())
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

        string interfaceName = contract.Name;
        string namespaceName = contract.ContainingNamespace.ToDisplayString();
        ImmutableArray<ProcedureInfo> procedures = contract.GetMembers()
            .Select(m => m as IMethodSymbol)
            .Where(m => m != null)
            .Select(m => new ProcedureInfo(m!))
            .ToImmutableArray();

        if (procedures.Length == 0)
        {
            return null;
        }

        return new ContractInfo(interfaceName, namespaceName, procedures, contractType.Value);
    }
}
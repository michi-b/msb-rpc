#region

using System;
using MsbRpc.Generator.Enums;

#endregion

namespace MsbRpc.Generator.Extensions;

internal static class ContractAccessibilityExtensions
{
    public static string GetKeyword(this ContractAccessibility target)
    {
        return target switch
        {
            ContractAccessibility.Internal => "internal",
            ContractAccessibility.Public => "public",
            _ => throw new ArgumentOutOfRangeException(nameof(target), target, null)
        };
    }
}
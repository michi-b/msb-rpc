#region

using System;
using Microsoft.CodeAnalysis;
using MsbRpc.Generator.Enums;

#endregion

namespace MsbRpc.Generator.Extensions;

public static class AccessibilityExtensions
{
    internal static bool TryGet(this Accessibility declaredAccessibility, out ContractAccessibility result)
    {
        switch (declaredAccessibility)
        {
            case Accessibility.NotApplicable:
            case Accessibility.Private:
            case Accessibility.ProtectedAndInternal:
            case Accessibility.ProtectedOrInternal:
            case Accessibility.Protected:
                break;
            case Accessibility.Internal:
                result = ContractAccessibility.Internal;
                return true;
            case Accessibility.Public:
                result = ContractAccessibility.Public;
                return true;
            default:
                throw new ArgumentOutOfRangeException(nameof(declaredAccessibility), declaredAccessibility, null);
        }

        result = default;
        return false;
    }
}
#region

using System.Collections.Generic;

#endregion

namespace MsbRpc.Generator.Info.Comparers;

internal class TargetComparer : IEqualityComparer<ContractInfo>
{
    public static TargetComparer Instance { get; } = new();

    public bool Equals(ContractInfo x, ContractInfo y)
        => x.InterfaceName == y.InterfaceName
           && x.Namespace == y.Namespace;

    public int GetHashCode(ContractInfo obj)
    {
        unchecked
        {
            int hashCode = obj.InterfaceName.GetHashCode();
            hashCode = (hashCode * 397) ^ obj.Namespace.GetHashCode();
            return hashCode;
        }
    }
}
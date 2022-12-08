namespace MsbRpc.Generator.Info;

public class ContractInfoTargetComparer : IEqualityComparer<ContractInfo>
{
    public static ContractInfoTargetComparer Instance { get; } = new();

    public bool Equals(ContractInfo x, ContractInfo y)
        => x.Name == y.Name
           && x.Namespace == y.Namespace;

    public int GetHashCode(ContractInfo obj)
    {
        unchecked
        {
            int hashCode = obj.Name.GetHashCode();
            hashCode = (hashCode * 397) ^ obj.Namespace.GetHashCode();
            return hashCode;
        }
    }
}
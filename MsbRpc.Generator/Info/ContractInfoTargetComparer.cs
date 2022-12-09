namespace MsbRpc.Generator.Info;

public partial class ContractInfo
{
    public class TargetComparer : IEqualityComparer<ContractInfo>
    {
        public static TargetComparer Instance { get; } = new();

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
}

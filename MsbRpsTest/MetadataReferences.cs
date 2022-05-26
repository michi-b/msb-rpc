using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace MsbRpsTest
{
    public class MetadataReferences
    {
        public static readonly ImmutableArray<MetadataReference> All = ImmutableArray.Create(Misbat.CodeAnalysis.Test.MetadataReferences.NetStandard);
    }
}
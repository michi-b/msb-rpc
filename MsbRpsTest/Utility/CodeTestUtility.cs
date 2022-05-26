using Misbat.CodeAnalysis.Test;
using Misbat.CodeAnalysis.Test.CodeTest;

namespace MsbRpsTest.Utility;

public static class CodeTestUtility
{
    public static readonly CodeTest Default = new(new CodeTestConfiguration(MetadataReferences.All));
}
#region

using Microsoft.CodeAnalysis;

#endregion

namespace MsbRpc.Generator.Info.Parsers;

internal static class ServerGenerationInfoParser
{
    public static ServerGenerationInfo? Parse(AttributeData generateServerAttribute) => new ServerGenerationInfo();
}
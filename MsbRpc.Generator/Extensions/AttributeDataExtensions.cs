using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace MsbRpc.Generator.Extensions;

public static class AttributeDataExtensions
{
    public static IEnumerable<KeyValuePair<string, TypedConstant>> GetArguments(this AttributeData data)
    {
        if (data.AttributeConstructor != null)
        {
            ImmutableArray<IParameterSymbol> parameters = data.AttributeConstructor.Parameters;
            string[] parameterNames = parameters.Select(parameter => parameter.Name).ToArray();
            for (int i = 0; i < data.ConstructorArguments.Length; i++)
            {
                yield return new KeyValuePair<string, TypedConstant>(parameterNames[i], data.ConstructorArguments[i]);
            }
        }

        foreach (KeyValuePair<string, TypedConstant> namedArgument in data.NamedArguments)
        {
            yield return namedArgument;
        }
    }
}
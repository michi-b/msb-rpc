using System.Text;

namespace MsbRpc.Generator.GenerationHelpers.Extensions;

public static class ParameterGeneratorArrayExtensions
{
    public static string GetString(this ParameterGenerator[] parameters)
    {
        if (parameters.Length == 0)
        {
            return string.Empty;
        }

        string firstParameterCode = parameters[0].ParameterCode;
        var stringBuilder = new StringBuilder((firstParameterCode.Length * parameters.Length * 2) 
                                              + 2 * (parameters.Length - 1)); //for commas and spaces 

        stringBuilder.Append(firstParameterCode);
        for (int i = 1; i < parameters.Length; i++)
        {
            stringBuilder.Append(", ");
            stringBuilder.Append(parameters[i].ParameterCode);
        }
        
        return stringBuilder.ToString();
    }
}
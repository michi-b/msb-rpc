namespace MsbRps.Generator.Exceptions;

public class SymbolCannotBeResolvedException : Exception
{
    public string SymbolName { get; }

    public SymbolCannotBeResolvedException(string symbolName)
        : base($"symbol {{{symbolName}}} cannot be resolved")
    {
        SymbolName = symbolName;
    }
}
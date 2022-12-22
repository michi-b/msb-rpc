namespace MsbRpc.Generator.GenerationHelpers.Names;

public class EndPointNames
{
    public static class Types
    {
        public const string EndPointBaseType = "MsbRpc.EndPoints.RpcEndPoint";
        public const string UndefinedProcedureEnum = "MsbRpc.EndPoints.UndefinedProcedure";
    }
    
    public static class Methods
    {
        public const string EnterCalling = "EnterCalling";
        public const string ExitCalling = "ExitCalling";
        public const string GetRequestWriter = "GetRequestWriter";
        public const string GetResponseWriter = "GetResponseWriter";
    }
    
    public static class Parameters
    {
        public const string BufferSize = "initialBufferSize";
    }
    
    public static class Fields
    {
        public const string DefaultBufferSizeConstant = "DefaultBufferSize";
    }

    /// <summary>{ContractNamespace}.Generated.{Contract}{Client/Server}Endpoint.g.cs</summary>
    public string EndPointFile { get; }

    /// <summary>{ContractNamespace}.Generated.I{Contract}{Client/Server}.g.cs</summary>
    public string InterfaceFile { get; }

    /// <summary>{ContractNamespace}.Generated.{Contract}{Client/Server}Procedure.g.cs</summary>
    public string ProcedureFile { get; }

    /// <summary>{ContractNamespace}.Generated.{Contract}{Client/Server}ProcedureExtensions.g.cs</summary>
    public string ProcedureExtensionsFile { get; }

    /// <summary>_{contract}{Client/Server}</summary>
    public string InterfaceField { get; }

    /// <summary>{contract}{Client/Server}</summary>
    public string InterfaceParameter { get; }

    /// <summary>I{Contract}{Client/Server}</summary>
    public string Interface { get; }

    /// <summary>{Contract}{Client/Server}Procedure</summary>
    public string ProcedureEnum { get; }

    /// <summary>{Contract}{Client/Server}ProcedureExtensions</summary>
    public string ProcedureEnumExtensions { get; }

    /// <summary>{Contract}{Client/Server}Endpoint</summary>
    public string EndPointType { get; }

    public EndPointNames(string generatedNamespace, string contractName, string lowerCaseContractName, EndPointId endPointType)
    {
        string typeName = endPointType.GetName();
        
        InterfaceField = $"_{lowerCaseContractName}{typeName}";
        InterfaceParameter = $"{lowerCaseContractName}{typeName}";

        Interface = $"{GeneralNames.InterfacePrefix}{contractName}{typeName}";
        InterfaceFile = $"{generatedNamespace}.{Interface}{GeneralNames.GeneratedFileEnding}";

        ProcedureEnum = $"{contractName}{typeName}{ContractNames.ProcedurePostfix}";
        ProcedureFile = $"{generatedNamespace}.{ProcedureEnum}{GeneralNames.GeneratedFileEnding}";

        ProcedureEnumExtensions = $"{contractName}{typeName}{ContractNames.ProcedurePostfix}Extensions";
        ProcedureExtensionsFile = $"{generatedNamespace}.{ProcedureEnumExtensions}{GeneralNames.GeneratedFileEnding}";

        EndPointType = $"{contractName}{typeName}EndPoint";
        EndPointFile = $"{generatedNamespace}.{EndPointType}{GeneralNames.GeneratedFileEnding}";
    }
}
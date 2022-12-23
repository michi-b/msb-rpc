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
        public const string ArgumentsBufferReader = "arguments";
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
    public string InboundProcedureEnum { get; }
    
    /// <summary>{Contract}{client/server}Procedure</summary>
    public string InboundProcedureEnumParameter { get; }

    /// <summary>{Contract}{Client/Server}ProcedureExtensions</summary>
    public string ProcedureEnumExtensions { get; }

    /// <summary>{Contract}{Client/Server}Endpoint</summary>
    public string EndPointType { get; }

    public EndPointNames(string generatedNamespace, string contractName, EndPointId endPointType)
    {
        string endPointTypeName = endPointType.GetName();
        string lowerCaseEndPointTypeName = endPointType.GetLowerCaseName();
        
        string lowerCaseContractName = contractName.WithLowerFirstChar();
        
        InterfaceField = $"_{lowerCaseContractName}{endPointTypeName}";
        InterfaceParameter = $"{lowerCaseContractName}{endPointTypeName}";

        Interface = $"{GeneralNames.InterfacePrefix}{contractName}{endPointTypeName}";
        InterfaceFile = $"{generatedNamespace}.{Interface}{GeneralNames.GeneratedFileEnding}";

        InboundProcedureEnum = $"{contractName}{endPointTypeName}{ContractNames.ProcedurePostfix}";
        ProcedureFile = $"{generatedNamespace}.{InboundProcedureEnum}{GeneralNames.GeneratedFileEnding}";
        InboundProcedureEnumParameter = $"{lowerCaseEndPointTypeName}{ContractNames.ProcedurePostfix}";
        
        ProcedureEnumExtensions = $"{contractName}{endPointTypeName}{ContractNames.ProcedurePostfix}Extensions";
        ProcedureExtensionsFile = $"{generatedNamespace}.{ProcedureEnumExtensions}{GeneralNames.GeneratedFileEnding}";

        EndPointType = $"{contractName}{endPointTypeName}EndPoint";
        EndPointFile = $"{generatedNamespace}.{EndPointType}{GeneralNames.GeneratedFileEnding}";
    }
}
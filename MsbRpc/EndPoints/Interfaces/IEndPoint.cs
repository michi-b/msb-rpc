namespace MsbRpc.EndPoints.Interfaces;

public interface IEndPoint
{
    /// <summary>
    ///     the id of the endpoint, usually given by the registry server counting up from 0 (and wrapping at int.MaxValue),
    ///     or -1, if no id was passed to the constructor
    /// </summary>
    public int Id { get; }
    
    public string Name { get; }
}
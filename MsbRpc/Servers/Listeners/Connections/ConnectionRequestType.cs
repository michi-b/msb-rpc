namespace MsbRpc.Servers.Listeners.Connections;

public enum ConnectionRequestType : byte
{
    /// <summary>
    ///     The connection has no specific target
    /// </summary>
    UnIdentified = 0,

    /// <summary>
    ///     The connection carries an id to link it to a waiting listen task
    /// </summary>
    Identified = 1
}
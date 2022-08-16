namespace MsbRpc.Messaging;

[Serializable]
internal class AlreadyListeningException : Exception
{
    public AlreadyListeningException() : base("trying to make a socketwrapper listen while it is already listening") { }
}
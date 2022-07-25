namespace MsbRpc.Messaging
{
    public class NoMessageAvailableException : System.Exception
    {
        public NoMessageAvailableException() : base("trying to consume a message while none is available") { }
    }
}

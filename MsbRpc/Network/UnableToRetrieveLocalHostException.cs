namespace MsbRpc.Network;

public class UnableToRetrieveLocalHostException : NetworkException
{
    public UnableToRetrieveLocalHostException() : base("Unable to retrieve local host address") { }
}
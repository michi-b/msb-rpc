namespace MsbRpc.EndPoints.Exceptions;

public class ThereAreNoRequestsDefinedException : InvalidOperationException
{
    public ThereAreNoRequestsDefinedException()
        : base("there are no requests defined for this endpoint") { }
}
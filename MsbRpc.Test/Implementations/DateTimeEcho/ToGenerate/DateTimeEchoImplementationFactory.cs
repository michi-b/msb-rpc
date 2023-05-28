namespace MsbRpc.Test.Implementations.DateTimeEcho.ToGenerate;

public class DateTimeEchoImplementationFactory : IFactory<IDateTimeEcho>
{
    private readonly Func<IDateTimeEcho> _factoryMethod;
    public DateTimeEchoImplementationFactory(Func<IDateTimeEcho> factoryMethod) => _factoryMethod = factoryMethod;

    public IDateTimeEcho Create() => _factoryMethod();
}
namespace MsbRpc.Test.Implementations.DateTimeEcho.ToGenerate;

public class DateTimeEchoImplementationByDelegateFactory : IFactory<IDateTimeEcho>
{
    public delegate IDateTimeEcho FactoryDelegate();

    private readonly FactoryDelegate _factoryMethod;
    private DateTimeEchoImplementationByDelegateFactory(FactoryDelegate factoryMethod) => _factoryMethod = factoryMethod;

    public static implicit operator DateTimeEchoImplementationByDelegateFactory(FactoryDelegate create) => new(create);
    public IDateTimeEcho Create() => _factoryMethod();
}
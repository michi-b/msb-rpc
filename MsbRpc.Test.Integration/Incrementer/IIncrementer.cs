using System.Diagnostics.SymbolStore;
using MsbRpc.Generator.Attributes;

namespace MsbRpc.Test.Integration.Incrementer;

[RpcContract]
public interface IIncrementer
{
    int Increment(int value1, int value2, int value3);
    int Decrement(int valueWhat);
    bool WhatsUpCrement(char valueWhatever);
}
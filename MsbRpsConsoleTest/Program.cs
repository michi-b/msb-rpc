// See https://aka.ms/new-console-template for more information

using NetEscapades.EnumGenerators;

BufferedStream stream = new BufferedStream(new MemoryStream());

[EnumExtensions]
public enum TestEnum
{
    Test1,
    Test2,
}
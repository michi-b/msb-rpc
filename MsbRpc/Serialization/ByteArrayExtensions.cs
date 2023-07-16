#region

using System.Text;

#endregion

namespace MsbRpc.Serialization;

public static class ByteArrayExtensions
{
    public static string CreateContentString(this byte[] target) => target.CreateContentString(0, target.Length);

    public static string CreateContentString(this byte[] target, int offset, int count)
    {
        var sb = new StringBuilder
        (
            "(",
            count * 3 // 3 chars per byte
            + (count - 1) * 2 // 2 chars per separator
            + 2
        ); // 2 chars for parentheses

        int end = offset + count;

        for (int i = offset; i < end - 1; i++)
        {
            sb.Append(target[i]);
            sb.Append(", ");
        }

        sb.Append(target[end - 1]);
        sb.Append(')');

        return sb.ToString();
    }
}
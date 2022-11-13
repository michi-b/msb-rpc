using System.Collections.Concurrent;

namespace MsbRpc.Concurrent;

public class UniqueIntProvider
{
    private readonly ConcurrentQueue<int> _values;

    public UniqueIntProvider(int minValue, int maxValue, bool shuffle)
    {
        int[] linear = new int[maxValue - minValue + 1];
        for (int i = 0; i < linear.Length; i++)
        {
            linear[i] = minValue + i;
        }

        if (shuffle)
        {
            var rnd = new Random();
            _values = new ConcurrentQueue<int>(linear.OrderBy(_ => rnd.Next()));
        }
        else
        {
            _values = new ConcurrentQueue<int>(linear);
        }
    }

    public int Get()
    {
        bool success = _values.TryDequeue(out int result);
        if (success)
        {
            return result;
        }

        throw new UniqueIntProviderRanOutOfOptionsException();
    }

    public void Return(int uniqueInt)
    {
        _values.Enqueue(uniqueInt);
    }
}
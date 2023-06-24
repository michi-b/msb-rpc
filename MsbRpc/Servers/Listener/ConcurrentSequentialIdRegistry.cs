using System;
using System.Collections.Concurrent;
using MsbRpc.Exceptions;

namespace MsbRpc.Servers.Listener;

public class ConcurrentSequentialIdRegistry<TItem> : IIdRegistry<TItem>
{
    private readonly ConcurrentDictionary<int, TItem> _items = new();

    private int _currentId;

    public int Push(TItem item, int maxRetries = 1000)
    {
        int id = -1;
        bool success = false;
        int retryCount = 0;

        while (retryCount < maxRetries)
        {
            lock (this)
            {
                id = _currentId++;

                if (!_items.ContainsKey(id))
                {
                    success = true;

                    if (!_items.TryAdd(id, item))
                    {
                        throw new InvalidOperationException($"Failed to add item with id {id}");
                    }

                    break;
                }

                retryCount++;
            }
        }

        if (!success)
        {
            throw new MaximumNumberOfRetriesExceededException(maxRetries);
        }

        return id;
    }

    public bool TryPop(int id, out TItem item) => _items.TryRemove(id, out item);
}
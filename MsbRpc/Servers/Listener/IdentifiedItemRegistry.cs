using System;
using System.Collections.Concurrent;

namespace MsbRpc.Servers.Listener;

public class IdentifiedItemRegistry<TItem>
{
    private readonly ConcurrentQueue<int> _freeIds = new();
    private readonly ConcurrentDictionary<int, TItem> _items = new();
    private int _maxId = -1;

    public int Add(TItem item)
    {
        int id;
        if (_freeIds.TryDequeue(out int reusedId))
        {
            id = reusedId;
        }
        else
        {
            lock (this)
            {
                id = _maxId++;
            }
        }

        if (!_items.TryAdd(id, item))
        {
            _freeIds.Enqueue(id);
            throw new InvalidOperationException($"Failed to add an item with id {id}");
        }

        return id;
    }

    public TItem Take(int id)
    {
        if (!_items.TryRemove(id, out TItem item))
        {
            throw new InvalidOperationException($"Failed ot take an item with id {id}");
        }

        _freeIds.Enqueue(id);

        return item;
    }
}
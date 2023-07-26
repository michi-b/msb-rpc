#region

using System;
using System.Collections.Concurrent;
using MsbRpc.Serialization.Buffers;
using MsbRpc.Serialization.Primitives;

#endregion

namespace MsbRpc.Servers.Listener;

public abstract class ConcurrentIdentifiedItemRegistry<TId, TItem> : IConcurrentIdentifiedItemRegistry<TId, TItem> where TId : struct
{
    private readonly ConcurrentQueue<TId> _freeIds = new();
    private readonly ConcurrentDictionary<TId, TItem> _items = new();

    public int MaxMessageSize => Message.Offset + PrimitiveSerializer.ByteSize + IdSize;

    protected abstract int IdSize { get; }

    public TId Add(TItem item)
    {
        TId id;
        if (_freeIds.TryDequeue(out TId reusedId))
        {
            id = reusedId;
        }
        else
        {
            lock (this)
            {
                id = AcquireNewId();
            }
        }

        if (!_items.TryAdd(id, item))
        {
            _freeIds.Enqueue(id);
            throw new InvalidOperationException($"Failed to add an item with id {id}");
        }

        return id;
    }

    public TItem Take(TId id)
    {
        if (!_items.TryRemove(id, out TItem item))
        {
            throw new InvalidOperationException($"Failed ot take an item with id {id}");
        }

        _freeIds.Enqueue(id);

        return item;
    }

    protected abstract TId AcquireNewId();
}
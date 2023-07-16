#region

using System.Collections.Immutable;

#endregion

namespace MsbRpc.Generator.Extensions;

internal static class ImmutableDictionaryBuilderExtensions
{
    // public static bool AddDistinct<TKey, TValue>(this ImmutableDictionary<TKey, TValue>.Builder builder, TKey key, TValue value)
    //     where TKey : notnull
    // {
    //     if (builder.ContainsKey(key))
    //     {
    //         return false;
    //     }
    //
    //     builder.Add(key, value);
    //     return true;
    // }

    public static void MirrorDistinct<TKey, TValue>(this ImmutableDictionary<TKey, TValue?>.Builder builder, ImmutableDictionary<TKey, TValue?> source, TKey key)
        where TKey : notnull
    {
        if (builder.ContainsKey(key))
        {
            return;
        }

        if (source.TryGetValue(key, out TValue? value))
        {
            builder.Add(key, value);
        }
    }
}
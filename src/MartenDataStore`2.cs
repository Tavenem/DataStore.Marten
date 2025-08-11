using Marten;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Text.Json.Serialization.Metadata;

namespace Tavenem.DataStorage.Marten;

/// <summary>
/// A data store for <typeparamref name="TItem"/> instances backed by a Marten implementation of
/// PostgreSQL.
/// </summary>
/// <typeparam name="TKey">The type of primary key for all stored items.</typeparam>
/// <typeparam name="TItem">A shared interface for all stored items.</typeparam>
/// <param name="documentStore">The <see cref="IDocumentStore"/> used for all transactions.</param>
/// <param name="cacheOptions">The options of the in-memory cache.</param>
public abstract class MartenDataStore<TKey, TItem>(
    IDocumentStore documentStore,
    IOptions<MemoryCacheOptions>? cacheOptions = null) : MartenDataStore<TItem>(documentStore), IDataStore<TKey, TItem>
    where TKey : notnull
    where TItem : notnull
{
    private readonly MemoryCache _cache = new(cacheOptions ?? new MemoryCacheOptions());

    /// <inheritdoc />
    public override TimeSpan DefaultCacheTimeout { get; set; } = TimeSpan.FromMinutes(10);

    /// <inheritdoc />
    public override bool SupportsCaching => true;

    /// <inheritdoc />
    public abstract TKey? CreateNewIdFor<T>() where T : TItem;

    /// <inheritdoc />
    public abstract TKey? CreateNewIdFor(Type type);

    /// <inheritdoc />
    public async ValueTask<T?> GetItemAsync<T>(TKey? id, TimeSpan? cacheTimeout = null, CancellationToken cancellationToken = default) where T : TItem
    {
        if (id is null)
        {
            return default;
        }
        return await _cache.GetOrCreateAsync(
            id,
            async entry =>
            {
                await using var session = DocumentStore.LightweightSession();
                return await session.LoadAsync<T>(id).ConfigureAwait(false);
            },
            new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = cacheTimeout ?? DefaultCacheTimeout
            }).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public ValueTask<T?> GetItemAsync<T>(
        TKey? id,
        JsonTypeInfo<T>? typeInfo,
        TimeSpan? cacheTimeout = null,
        CancellationToken cancellationToken = default) where T : TItem
        => GetItemAsync<T>(id, cacheTimeout, cancellationToken);

    /// <inheritdoc />
    public abstract TKey GetKey<T>(T item) where T : TItem;

    /// <inheritdoc />
    public abstract ValueTask<bool> RemoveItemAsync<T>(TKey? id, CancellationToken cancellationToken = default) where T : TItem;

    /// <inheritdoc />
    public override async ValueTask<T?> StoreItemAsync<T>(
        T? item,
        TimeSpan? cacheTimeout = null,
        CancellationToken cancellationToken = default) where T : default
    {
        var result = await base.StoreItemAsync(item, cacheTimeout, cancellationToken).ConfigureAwait(false);

        if (result is null)
        {
            return default;
        }

        _cache.Set(GetKey(result), item, cacheTimeout ?? DefaultCacheTimeout);

        return result;
    }
}

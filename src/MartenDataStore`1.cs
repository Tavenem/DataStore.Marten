using Marten;
using System.Text.Json.Serialization.Metadata;
using Tavenem.DataStorage.Interfaces;

namespace Tavenem.DataStorage.Marten;

/// <summary>
/// A data store for <typeparamref name="TItem"/> instances backed by a Marten implementation of
/// PostgreSQL.
/// </summary>
/// <typeparam name="TItem">A shared interface for all stored items.</typeparam>
/// <param name="documentStore">The <see cref="IDocumentStore"/> used for all transactions.</param>
public abstract class MartenDataStore<TItem>(IDocumentStore documentStore) : IDataStore<TItem> where TItem : notnull
{
    /// <inheritdoc />
    public virtual TimeSpan DefaultCacheTimeout { get; set; }

    /// <summary>
    /// The <see cref="IDocumentStore"/> used for all transactions.
    /// </summary>
    public IDocumentStore DocumentStore { get; set; } = documentStore;

    /// <inheritdoc />
    public virtual bool SupportsCaching => false;

    /// <inheritdoc />
    public IDataStoreQueryable<T> Query<T>(JsonTypeInfo<T>? typeInfo = null) where T : notnull, TItem
    {
        var session = DocumentStore.QuerySession();
        return new MartenDataStoreQueryable<TItem, T>(this, session, session.Query<T>());
    }

    /// <inheritdoc />
    public async ValueTask<bool> RemoveItemAsync<T>(T? item, CancellationToken cancellationToken = default) where T : TItem
    {
        if (item is null)
        {
            return true;
        }
        await using var session = DocumentStore.LightweightSession();
        session.Delete(item);
        await session.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return true;
    }

    /// <inheritdoc />
    public virtual async ValueTask<T?> StoreItemAsync<T>(T? item, TimeSpan? cacheTimeout = null, CancellationToken cancellationToken = default) where T : TItem
    {
        if (item is null)
        {
            return item;
        }
        await using var session = DocumentStore.LightweightSession();
        session.Store(item);
        await session.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return item;
    }

    /// <inheritdoc />
    public ValueTask<T?> StoreItemAsync<T>(T? item, JsonTypeInfo<T>? typeInfo, TimeSpan? cacheTimeout = null, CancellationToken cancellationToken = default) where T : TItem
        => StoreItemAsync(item, cacheTimeout, cancellationToken);
}

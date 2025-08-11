using Marten;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Tavenem.DataStorage.Marten;

/// <summary>
/// A data store for <see cref="IIdItem"/> instances backed by a Marten implementation of
/// PostgreSQL.
/// </summary>
/// <param name="documentStore">The <see cref="IDocumentStore"/> used for all transactions.</param>
/// <param name="cacheOptions">The options of the in-memory cache.</param>
public class MartenDataStore(
    IDocumentStore documentStore,
    IOptions<MemoryCacheOptions>? cacheOptions = null) : MartenDataStore<string, IIdItem>(documentStore, cacheOptions), IIdItemDataStore
{
    /// <inheritdoc />
    public override string? CreateNewIdFor<T>() => Guid.NewGuid().ToString();

    /// <inheritdoc />
    public override string? CreateNewIdFor(Type type) => Guid.NewGuid().ToString();

    /// <inheritdoc />
    public override string GetKey<T>(T item) => item.Id;

    /// <inheritdoc />
    public override async ValueTask<bool> RemoveItemAsync<T>(string? id, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(id))
        {
            return false;
        }
        await using var session = DocumentStore.LightweightSession();
        session.Delete<T>(id);
        await session.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return true;
    }
}

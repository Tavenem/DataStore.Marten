using Marten;
using Marten.Pagination;
using System.Linq.Expressions;
using System.Numerics;
using Tavenem.DataStorage.Interfaces;
using Tavenem.DataStorage.Marten;

namespace Tavenem.DataStorage;

/// <summary>
/// Provides LINQ operations on a <see cref="MartenDataStore{TItem}"/>'s data.
/// </summary>
/// <typeparam name="TItem">A shared interface for all stored items.</typeparam>
/// <typeparam name="TSource">
/// The type of the elements of the source.
/// </typeparam>
public class MartenDataStoreQueryable<TItem, TSource>(MartenDataStore<TItem> store, IQuerySession session, IQueryable<TSource> source)
    : IAsyncDisposable,
    IDataStoreDistinctQueryable<TSource>,
    IDataStoreFirstQueryable<TSource>,
    IDataStoreOrderableQueryable<TSource>,
    IDataStoreSelectManyQueryable<TSource>,
    IDataStoreSelectQueryable<TSource>,
    IDataStoreSkipQueryable<TSource>,
    IDataStoreTakeQueryable<TSource>,
    IDataStoreWhereQueryable<TSource>,
    IDisposable
    where TItem : notnull
    where TSource : notnull
{
    /// <summary>
    /// The underlying store.
    /// </summary>
    protected readonly MartenDataStore<TItem> store = store;

    /// <summary>
    /// The underlying Marten <see cref="IQuerySession"/>.
    /// </summary>
    protected readonly IQuerySession session = session;

    /// <summary>
    /// The underlying <see cref="IQueryable{T}"/> source.
    /// </summary>
    protected readonly IQueryable<TSource> source = source;

    /// <inheritdoc />
    public IDataStore Provider => store;

    /// <inheritdoc />
    public async ValueTask<TResult> AverageAsync<TResult>(
        Expression<Func<TSource, TResult?>> selector,
        CancellationToken cancellationToken = default) where TResult : INumberBase<TResult>
#pragma warning disable CS8714 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'notnull' constraint.
        // Suppressed to implement interface successfully. If the relaxed type constraint results in a runtime failure,
        // the user is expected to deal with the resulting exception.
        => TResult.CreateChecked(await source.AverageAsync(selector, cancellationToken));
#pragma warning restore CS8714 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'notnull' constraint.

    /// <summary>
    /// Determines whether a sequence contains any elements.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>
    /// <see langword="true"/> if the source sequence contains any elements; otherwise, <see
    /// langword="false"/>.
    /// </returns>
    public async ValueTask<bool> AnyAsync(CancellationToken cancellationToken)
        => await source.AnyAsync(cancellationToken);

    /// <summary>
    /// Determines whether any element of a sequence satisfies a condition.
    /// </summary>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>
    /// <see langword="true"/>> if the source sequence is not empty and at least one of its elements
    /// passes the test in the specified predicate; otherwise, <see langword="false"/>.
    /// </returns>
    public async ValueTask<bool> AnyAsync(Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken)
        => await source.AnyAsync(predicate, cancellationToken);

    /// <summary>
    /// Returns the number of elements in this source.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>The number of elements in the this source.</returns>
    /// <exception cref="OverflowException">
    /// The number of elements in this source is larger than <see cref="int.MaxValue"/>.
    /// </exception>
    public async ValueTask<int> CountAsync(CancellationToken cancellationToken)
        => await source.CountAsync(cancellationToken);

    /// <summary>
    /// Returns the number of elements in this source that satisfy a condition.
    /// </summary>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>The number of elements in the this source.</returns>
    /// <exception cref="OverflowException">
    /// The number of elements in this source that satisfy the condition is larger than <see
    /// cref="int.MaxValue"/>.
    /// </exception>
    public async ValueTask<int> CountAsync(Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken)
        => await source.CountAsync(predicate, cancellationToken);

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        await DisposeAsyncCore().ConfigureAwait(false);

        Dispose(disposing: false);
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc />
    public IDataStoreDistinctQueryable<TSource> Distinct(IEqualityComparer<TSource>? comparer = null)
        => new MartenDataStoreQueryable<TItem, TSource>(store, session, source.Distinct(comparer));

    /// <inheritdoc />
    public async ValueTask<TSource> FirstAsync(CancellationToken cancellationToken = default)
        => await source.FirstAsync(cancellationToken);

    /// <inheritdoc />
    public async ValueTask<TSource> FirstAsync(Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default)
        => await source.FirstAsync(predicate, cancellationToken);

    /// <inheritdoc />
    public async ValueTask<TSource?> FirstOrDefaultAsync(CancellationToken cancellationToken = default)
        => await source.FirstOrDefaultAsync(cancellationToken);

    /// <inheritdoc />
    public async ValueTask<TSource?> FirstOrDefaultAsync(Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default)
        => await source.FirstOrDefaultAsync(predicate, cancellationToken);

    /// <summary>
    /// Returns the number of elements in this source.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>The number of elements in the this source.</returns>
    /// <exception cref="OverflowException">
    /// The number of elements in this source is larger than <see cref="long.MaxValue"/>.
    /// </exception>
    public async ValueTask<long> LongCountAsync(CancellationToken cancellationToken)
        => await source.CountAsync(cancellationToken);

    /// <summary>
    /// Returns the number of elements in this source that satisfy a condition.
    /// </summary>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>The number of elements in the this source.</returns>
    /// <exception cref="OverflowException">
    /// The number of elements in this source that satisfy the condition is larger than <see
    /// cref="long.MaxValue"/>.
    /// </exception>
    public async ValueTask<long> LongCountAsync(Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken)
        => await source.CountAsync(predicate, cancellationToken);

    /// <inheritdoc />
    public async ValueTask<TResult?> MaxAsync<TResult>(
        Expression<Func<TSource, TResult>> selector,
        CancellationToken cancellationToken)
#pragma warning disable CS8714 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'notnull' constraint.
        // Suppressed to implement interface successfully. If the relaxed type constraint results in a runtime failure,
        // the user is expected to deal with the resulting exception.
        => await source.MaxAsync(selector, cancellationToken);
#pragma warning restore CS8714 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'notnull' constraint.

    /// <inheritdoc />
    public async ValueTask<TResult?> MinAsync<TResult>(
        Expression<Func<TSource, TResult>> selector,
        CancellationToken cancellationToken = default)
#pragma warning disable CS8714 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'notnull' constraint.
        // Suppressed to implement interface successfully. If the relaxed type constraint results in a runtime failure,
        // the user is expected to deal with the resulting exception.
        => await source.MinAsync(selector, cancellationToken);
#pragma warning restore CS8714 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'notnull' constraint.

    /// <inheritdoc />
    IAsyncEnumerator<TSource> IAsyncEnumerable<TSource>.GetAsyncEnumerator(CancellationToken cancellationToken)
        => source.ToAsyncEnumerable(cancellationToken).GetAsyncEnumerator(cancellationToken);

    /// <summary>
    /// Returns an <see cref="IEnumerator{T}"/> for this source. The enumerator provides a simple
    /// way to access all the contents of the collection.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>An <see cref="IEnumerable{T}" />.</returns>
    /// <exception cref="OperationCanceledException">
    /// If the <see cref="CancellationToken"/> is cancelled.
    /// </exception>
    public ValueTask<IEnumerator<TSource>> GetEnumeratorAsync(CancellationToken cancellationToken = default)
        => new(source.GetEnumerator());

    /// <inheritdoc />
    public async ValueTask<IPagedList<TSource>> GetPageAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
        => (await source.ToPagedListAsync(pageNumber, pageSize, cancellationToken)).AsPagedList();

    /// <inheritdoc />
    public IOrderedDataStoreQueryable<TSource> Order(IComparer<TSource>? comparer = null)
        => comparer is null
        ? new OrderedMartenDataStoreQueryable<TItem, TSource>(store, session, source.Order())
        : new OrderedMartenDataStoreQueryable<TItem, TSource>(store, session, source.Order(comparer));

    /// <inheritdoc />
    public IOrderedDataStoreQueryable<TSource> OrderBy<TKey>(Expression<Func<TSource, TKey>> keySelector, IComparer<TKey>? comparer = null)
        => new OrderedMartenDataStoreQueryable<TItem, TSource>(store, session, source.OrderBy(keySelector, comparer));

    /// <inheritdoc />
    public IOrderedDataStoreQueryable<TSource> OrderByDescending<TKey>(Expression<Func<TSource, TKey>> keySelector, IComparer<TKey>? comparer = null)
        => new OrderedMartenDataStoreQueryable<TItem, TSource>(store, session, source.OrderByDescending(keySelector, comparer));

    /// <inheritdoc />
    public IOrderedDataStoreQueryable<TSource> OrderDescending(IComparer<TSource>? comparer = null)
        => comparer is null
        ? new OrderedMartenDataStoreQueryable<TItem, TSource>(store, session, source.OrderDescending())
        : new OrderedMartenDataStoreQueryable<TItem, TSource>(store, session, source.OrderDescending(comparer));

    /// <inheritdoc />
    public async ValueTask<TSource> SingleAsync(CancellationToken cancellationToken = default)
        => await source.SingleAsync(cancellationToken);

    /// <inheritdoc />
    public async ValueTask<TSource> SingleAsync(Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default)
        => await source.SingleAsync(predicate, cancellationToken);

    /// <inheritdoc />
    public async ValueTask<TSource?> SingleOrDefaultAsync(CancellationToken cancellationToken = default)
        => await source.SingleOrDefaultAsync(cancellationToken);

    /// <inheritdoc />
    public async ValueTask<TSource?> SingleOrDefaultAsync(Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default)
        => await source.SingleOrDefaultAsync(predicate, cancellationToken);

    /// <inheritdoc />
    public IDataStoreSelectQueryable<TResult> Select<TResult>(Expression<Func<TSource, TResult>> selector) where TResult : notnull
        => new MartenDataStoreQueryable<TItem, TResult>(store, session, source.Select(selector));

    /// <inheritdoc />
    public IDataStoreSelectQueryable<TResult> Select<TResult>(Expression<Func<TSource, int, TResult>> selector) where TResult : notnull
        => new MartenDataStoreQueryable<TItem, TResult>(store, session, source.Select(selector));

    /// <inheritdoc />
    public IDataStoreSelectManyQueryable<TResult> SelectMany<TCollection, TResult>(
        Expression<Func<TSource, IEnumerable<TCollection>>> collectionSelector,
        Expression<Func<TSource, TCollection, TResult>> resultSelector) where TResult : notnull
        => new MartenDataStoreQueryable<TItem, TResult>(store, session, source.SelectMany(collectionSelector, resultSelector));

    /// <inheritdoc />
    public IDataStoreSelectManyQueryable<TResult> SelectMany<TCollection, TResult>(
        Expression<Func<TSource, int, IEnumerable<TCollection>>> collectionSelector,
        Expression<Func<TSource, TCollection, TResult>> resultSelector) where TResult : notnull
        => new MartenDataStoreQueryable<TItem, TResult>(store, session, source.SelectMany(collectionSelector, resultSelector));

    /// <inheritdoc />
    public IDataStoreSkipQueryable<TSource> Skip(int count)
        => new MartenDataStoreQueryable<TItem, TSource>(store, session, source.Skip(count));

    /// <inheritdoc />
    public async ValueTask<TResult> SumAsync<TResult>(
        Expression<Func<TSource, TResult?>> selector,
        CancellationToken cancellationToken = default) where TResult : INumberBase<TResult>
#pragma warning disable CS8714 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'notnull' constraint.
        // Suppressed to implement interface successfully. If the relaxed type constraint results in a runtime failure,
        // the user is expected to deal with the resulting exception.
        => await source.SumAsync(selector, cancellationToken) ?? TResult.Zero;
#pragma warning restore CS8714 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'notnull' constraint.

    /// <inheritdoc />
    public IDataStoreTakeQueryable<TSource> Take(int count)
        => new MartenDataStoreQueryable<TItem, TSource>(store, session, source.Take(count));

    /// <inheritdoc />
    public IDataStoreTakeQueryable<TSource> Take(Range range)
        => new MartenDataStoreQueryable<TItem, TSource>(store, session, source.Skip(range.Start.Value).Take(range.End.Value - range.Start.Value));

    /// <summary>
    /// Creates a list from this source.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A list that contains the elements from this source.</returns>
    public async ValueTask<List<TSource>> ToListAsync(CancellationToken cancellationToken = default)
        => [.. await source.ToListAsync(cancellationToken)];

    /// <inheritdoc />
    public async ValueTask<(bool Success, int Count)> TryGetNonEnumeratedCountAsync(CancellationToken cancellationToken = default)
    {
        var result = await source.CountAsync(cancellationToken);
        return (true, result);
    }

    /// <inheritdoc />
    public async ValueTask<(bool Success, long Count)> TryGetNonEnumeratedLongCountAsync(CancellationToken cancellationToken = default)
    {
        var result = await source.LongCountAsync(cancellationToken);
        return (true, result);
    }

    /// <inheritdoc />
    public IDataStoreWhereQueryable<TSource> Where(Expression<Func<TSource, bool>> predicate)
        => new MartenDataStoreQueryable<TItem, TSource>(store, session, source.Where(predicate));

    /// <inheritdoc />
    public IDataStoreWhereQueryable<TSource> Where(Expression<Func<TSource, int, bool>> predicate)
        => new MartenDataStoreQueryable<TItem, TSource>(store, session, source.Where(predicate));

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting
    /// unmanaged resources.
    /// </summary>
    /// <param name="disposing">Whether to dispose managed resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            session.Dispose();
        }
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or
    /// resetting unmanaged resources asynchronously.
    /// </summary>
    protected virtual async ValueTask DisposeAsyncCore()
        => await session.DisposeAsync().ConfigureAwait(false);
}

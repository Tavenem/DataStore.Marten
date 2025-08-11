using Marten;
using System.Linq.Expressions;
using Tavenem.DataStorage.Interfaces;

namespace Tavenem.DataStorage.Marten;

/// <summary>
/// Provides LINQ operations on an <see cref="MartenDataStore{TItem}"/>, after an ordering
/// operation.
/// </summary>
/// <typeparam name="TItem">A shared interface for all stored items.</typeparam>
/// <typeparam name="TSource">
/// The type of the elements of the source.
/// </typeparam>
public class OrderedMartenDataStoreQueryable<TItem, TSource>(
    MartenDataStore<TItem> store,
    IQuerySession session,
    IOrderedQueryable<TSource> source)
    : MartenDataStoreQueryable<TItem, TSource>(store, session, source),
    IOrderedDataStoreQueryable<TSource>
    where TItem : notnull
    where TSource : notnull
{
    /// <summary>
    /// The underlying <see cref="IOrderedQueryable{T}"/> source.
    /// </summary>
    protected readonly IOrderedQueryable<TSource> orderedSource = source;

    /// <inheritdoc />
    public IOrderedDataStoreQueryable<TSource> ThenBy<TKey>(Expression<Func<TSource, TKey>> keySelector, IComparer<TKey>? comparer = null)
        => new OrderedMartenDataStoreQueryable<TItem, TSource>(store, session, orderedSource.ThenBy(keySelector, comparer));

    /// <inheritdoc />
    public IOrderedDataStoreQueryable<TSource> ThenByDescending<TKey>(Expression<Func<TSource, TKey>> keySelector, IComparer<TKey>? comparer = null)
        => new OrderedMartenDataStoreQueryable<TItem, TSource>(store, session, orderedSource.ThenByDescending(keySelector, comparer));
}

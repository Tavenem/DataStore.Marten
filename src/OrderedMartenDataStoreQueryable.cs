﻿using Marten;
using System.Linq.Expressions;

namespace Tavenem.DataStorage.Marten;

/// <summary>
/// Provides LINQ operations on a <see cref="MartenDataStore"/>, after an ordering operation..
/// </summary>
public class OrderedMartenDataStoreQueryable<T>(IQuerySession session, IOrderedQueryable<T> source)
    : MartenDataStoreQueryable<T>(session, source), IOrderedDataStoreQueryable<T>
{
    /// <summary>
    /// Performs a subsequent ordering of the elements in this <see
    /// cref="IOrderedDataStoreQueryable{T}"/> in the given order.
    /// </summary>
    /// <typeparam name="TKey">The type of the key returned by the function represented by
    /// <paramref name="keySelector"/>.</typeparam>
    /// <param name="keySelector">A function to extract a key from each element.</param>
    /// <param name="descending">Whether results will be ordered in descending order.</param>
    /// <returns>
    /// An <see cref="IOrderedDataStoreQueryable{T}"/> whose elements are sorted according to a
    /// key.
    /// </returns>
    public IOrderedDataStoreQueryable<T> ThenBy<TKey>(Expression<Func<T, TKey>> keySelector, bool descending = false)
        => descending
        ? new OrderedMartenDataStoreQueryable<T>(_session, ((IOrderedQueryable<T>)_source).ThenByDescending(keySelector))
        : new OrderedMartenDataStoreQueryable<T>(_session, ((IOrderedQueryable<T>)_source).ThenBy(keySelector));
}

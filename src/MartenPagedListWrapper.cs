using System.Collections;
using MartenPagination = Marten.Pagination;

namespace Tavenem.DataStorage.Marten;

/// <summary>
/// Initializes a new instance of the <see cref="PagedList{T}"/> class that contains elements copied
/// from the specified collection and has sufficient capacity to accommodate the number of elements
/// copied.
/// </summary>
/// <param name="list">
/// The collection whose elements are copied to the new list.
/// </param>
internal class MartenPagedListWrapper<T>(MartenPagination.IPagedList<T> list) : IPagedList<T>
{
    /// <summary>
    /// Return the paged query result.
    /// </summary>
    /// <param name="index">Index to fetch item from paged query result.</param>
    /// <returns>Item from paged query result.</returns>
    public T this[int index] => list[index];

    /// <summary>
    /// The number of records in the paged query result.
    /// </summary>
    public int Count => list.Count <= int.MaxValue
        ? (int)list.Count
        : int.MaxValue;

    /// <summary>
    /// The zero-based index of the first item in the current page, within the whole collection.
    /// </summary>
    public long FirstIndexOnPage => list.FirstItemOnPage - 1;

    /// <summary>
    /// Whether there is next page available.
    /// </summary>
    public bool HasNextPage => list.HasNextPage;

    /// <summary>
    /// Whether there is a previous page available.
    /// </summary>
    public bool HasPreviousPage => list.HasPreviousPage;

    /// <summary>
    /// The zero-based index of the last item in the current page, within the whole collection.
    /// </summary>
    public long LastIndexOnPage => list.LastItemOnPage - 1;

    /// <summary>
    /// <para>
    /// The current page number.
    /// </para>
    /// <para>
    /// The first page is 1.
    /// </para>
    /// </summary>
    public long PageNumber => list.PageNumber;

    /// <summary>
    /// The page size.
    /// </summary>
    public long PageSize => list.PageSize;

    /// <summary>
    /// The total number of results, of which this page is a subset.
    /// </summary>
    public long? TotalCount => list.TotalItemCount;

    /// <summary>
    /// The total number of pages.
    /// </summary>
    public long? TotalPages => list.PageCount;

    /// <summary>
    /// Returns an enumerator that iterates through the <see cref="PagedList{T}"/>.
    /// </summary>
    /// <returns>A <see cref="List{T}.Enumerator"/> for the <see cref="PagedList{T}"/>.</returns>
    public IEnumerator<T> GetEnumerator() => list.GetEnumerator();

    /// <summary>
    /// Returns an enumerator that iterates through the <see cref="PagedList{T}"/>.
    /// </summary>
    /// <returns>A <see cref="List{T}.Enumerator"/> for the <see cref="PagedList{T}"/>.</returns>
    IEnumerator IEnumerable.GetEnumerator() => list.GetEnumerator();
}

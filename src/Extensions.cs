namespace Tavenem.DataStorage.Marten;

/// <summary>
/// Extension methods.
/// </summary>
public static class DataStorageMartenExtensions
{
    /// <summary>
    /// Converts the Marten implementation of a <see
    /// cref="global::Marten.Pagination.IPagedList{T}"/> into a <see cref="PagedList{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of items in the collection.</typeparam>
    /// <param name="pagedList">The Marten <c>Marten.Pagination.IPagedList{T}</c>.</param>
    /// <returns>
    /// A <see cref="PagedList{T}"/> containing the items in the current collection.
    /// </returns>
    public static PagedList<T> AsPagedList<T>(this global::Marten.Pagination.IPagedList<T> pagedList)
        => pagedList.AsPagedList(pagedList.PageNumber, pagedList.PageSize, pagedList.TotalItemCount);
}

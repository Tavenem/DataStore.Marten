# Changelog

## 2.0-preview.1
### Changed
- Update to .NET 10
- The original `MartenDataStore` has been divided into three separate implementations:
  - `MartenDataStore<TItem>` which is an abstract class that allows specifying the item type for stored items
    - `MartenDataStore<TItem>` implements the updated `IDataStore<TItem>` interface (see [the `Tavenem.DataStore` project](https://github.com/Tavenem/DataStore) for details)
  - `MartenDataStore<TKey, TItem>` which is an abstract class that extends `MartenDataStore<TItem>` and allows specifying the key types for stored items
    - `MartenDataStore<TKey, TItem>` implements the updated `IDataStore<TKey, TItem>` interface (see [the `Tavenem.DataStore` project](https://github.com/Tavenem/DataStore) for details)
  - `MartenDataStore` which replicates the original by extending `MartenDataStore<string, IIdItem>`
    - `MartenDataStore` implements the updated `IIdItemDataStore` interface (see [the `Tavenem.DataStore` project](https://github.com/Tavenem/DataStore) for details)
- `MartenDataStoreQueryable<TItem, TSource>` implements the following updated interfaces (see [the `Tavenem.DataStore` project](https://github.com/Tavenem/DataStore) for details):
  - `IDataStoreDistinctQueryable<TSource>`
  - `IDataStoreFirstQueryable<TSource>`
  - `IDataStoreOrderableQueryable<TSource>`
  - `IDataStoreSelectManyQueryable<TSource>`
  - `IDataStoreSelectQueryable<TSource>`
  - `IDataStoreSkipQueryable<TSource>`
  - `IDataStoreTakeQueryable<TSource>`
  - `IDataStoreWhereQueryable<TSource>`

## 1.0
### Changed
- Update dependencies
- Implement new interface API
- Update to .NET 8

## 0.34.1-preview
### Changed
- Clarify 1-based indexing of page numbers in `IPagedList`.

## 0.32.0-preview - 0.34.0-preview
### Updated
- Update dependencies

## 0.31.0-preview
### Changed
- Update to .NET 7 preview

## 0.30.0-preview
### Changed
- Update to .NET 6 preview
- Update to C# 10 preview

## 0.29.1-preview
### Added
- Initial preview release
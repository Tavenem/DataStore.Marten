![build](https://img.shields.io/github/actions/workflow/status/Tavenem/DataStore.Marten/publish.yml) [![NuGet downloads](https://img.shields.io/nuget/dt/Tavenem.DataStore.Marten)](https://www.nuget.org/packages/Tavenem.DataStore.Marten/)

Tavenem.DataStore.Marten
==

[Tavenem.DataStore](https://github.com/Tavenem/DataStore) is a persistence-agnostic repository library. Its intended purpose is to help author libraries which need to interact with a project's data layer, while remaining fully decoupled from persistence choices.

For example: you might want to author a library which can retrieve an object from the data store by ID, modify it, then update the item in the data store. You want your library to be useful to people who use [EntityFramework](https://docs.microsoft.com/en-us/ef/) to access a SQL database, people who use [Marten](https://martendb.io/) to access a PostgreSQL database, or people who work with [Azure Cosmos DB](https://azure.microsoft.com/en-us/services/cosmos-db/).

Tavenem.DataStore provides another possible way to handle this scenario. It provides a simple interface which encapsulates common data operations. As the author of a library, you can accept this interface and use it for all data operations. As a consumer of a library which uses Tavenem.DataStore.Marten, you can provide an implementation of this interface designed to work with the particular ORM or data storage SDK you are using in your project.

Tavenem.DataStore.Marten provides an implementation of the `IDataStore` library for [Marten](https://martendb.io/). Include it when your code depends on a library that uses Tavenem.DataStore, and your data storage layer uses Marten. A `MartenDataStore` object can be instantiated and provided wherever the library requires an implementation of `IDataStore`.

## Installation

Tavenem.DataStore.Marten is available as a [NuGet package](https://www.nuget.org/packages/Tavenem.DataStore.Marten/).

## Roadmap

New versions of Tavenem.DataStore.Marten should be expected whenever the API surface of the Tavenem [DataStore library](https://github.com/Tavenem/DataStore) receives an update.

There is a preview release available now.

Other updates to resolve bugs or add new features may occur at any time.

## Contributing

Contributions are always welcome. Please carefully read the [contributing](docs/CONTRIBUTING.md) document to learn more before submitting issues or pull requests.

## Code of conduct

Please read the [code of conduct](docs/CODE_OF_CONDUCT.md) before engaging with our community, including but not limited to submitting or replying to an issue or pull request.
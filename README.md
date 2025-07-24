# Clean & Onion Architecture Solution Template



The goal of this template is to provide a straightforward and efficient approach to enterprise application development, leveraging the power of Clean & Onion Architecture and ASP.NET Core. Using this template, you can effortlessly create a Single Page App (SPA) with ASP.NET Core and Angular , while adhering to the principles of Clean & Onion  Architecture. Getting started is easy - simply install the **.NET template** (see below for full details).

If you find this project useful, please give it a star. Thanks! ⭐

## Getting Started

The following prerequisites are required to build and run the solution:

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) (latest version)
- [Node.js](https://nodejs.org/) (latest LTS, only required if you are using Angular or React)

The easiest way to get started is to install the [.NET template](https://www.nuget.org/packages/Clean.Architecture.Solution.Template):
```
dotnet new install Clean.Architecture.Solution.Template::9.0.1
```

Once installed, create a new solution using the template. You can choose to use Angular, React, or create a Web API-only solution. Specify the client framework using the `-cf` or `--client-framework` option, and provide the output directory where your project will be created. Here are some examples:

To create a ASP.NET Core Web API-only solution:
```bash
dotnet new arabdt-template -na YourProjectName
```

Launch the app:
```bash
cd src/api
dotnet run
```

To learn more, run the following command:
```bash
dotnet new arabdt-template --help
```

You can create fully use cases (commands & queries) or () by navigating to `cd src` and running `dotnet new ca --help` or `dotnet new oa --help` . Here are some examples:

To create a new command:
```bash
dotnet new ca-usecase --name CreateTodoList --feature-name TodoLists --usecase-type command --return-type int
```

To create a query:
```bash
dotnet new ca-usecase -n GetTodos -fn TodoLists -ut query -rt TodosVm
```

To learn more, run the following command:
```bash
dotnet new ca-usecase --help
```

## Database

The template is configured to use SQL Server by default. If you would prefer to use SQLite, create your solution using the following command:

```bash
dotnet new ca-sln --use-sqlite
```

When you run the application the database will be automatically created (if necessary) and the latest migrations will be applied.

Running database migrations is easy. Ensure you add the following flags to your command (values assume you are executing from repository root)

* `--project src/Infrastructure` (optional if in this folder)
* `--startup-project src/Web`
* `--output-dir Data/Migrations`

For example, to add a new migration from the root folder:

 `dotnet ef migrations add "SampleMigration" --project src\Infrastructure --startup-project src\Web --output-dir Data\Migrations`

## Deploy


## Technologies

* [ASP.NET Core 9](https://docs.microsoft.com/en-us/aspnet/core/introduction-to-aspnet-core)
* [Entity Framework Core 9](https://docs.microsoft.com/en-us/ef/core/)
* [Angular 18](https://angular.dev/) or [React 18](https://react.dev/)
* [MediatR](https://github.com/jbogard/MediatR)
* [AutoMapper](https://automapper.org/)
* [FluentValidation](https://fluentvalidation.net/)
* [NUnit](https://nunit.org/), [FluentAssertions](https://fluentassertions.com/), [Moq](https://github.com/devlooped/moq) & [Respawn](https://github.com/jbogard/Respawn)

## Versions
The main branch is now on .NET 9.0. The following previous versions are available:

* [8.0](https://gitlab.com/AElbaradey/arabdt.template/tree/test)

## Learn More

 
## Support

If you are having problems, please let me know by [raising a new issue](https://gitlab.com/AElbaradey/arabdt.template/-/issues/new).

## License

This project is licensed with the [MIT license](LICENSE).

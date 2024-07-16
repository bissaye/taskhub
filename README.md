# TaskHub API

This repository contains a task management API developed using ASP.NET Core. The API allows for creating, updating, retrieving, and deleting tasks for various projects within an organization.

## Project Architecture

The project is divided into several components:

- **TaskHub**: The main project containing API controllers and application configuration.
  - **Controllers**: API controllers to handle HTTP requests.
  - **Extensions**: Extensions and service registration methods for the application.
  - **Migrations**: Database migration files.

- **TaskHub.Business**: The project containing business logic for the API.
  - **Models**: Data models used by the business logic.
  - **Services**: Business services to handle complex operations.
  - **UseCases**: Use cases implementing specific business logic.
  - **ExceptionHandler**: Handle all errors in app and return specifics responses

- **TaskHub.Cache**: The project containing cache management.
  
- **TaskHub.Data**: The project containing data access and database management.
  - **Models**: Data models and entities used by data access.
  - **Repositories**: Repositories for data access and manipulation.

## Prerequisites

Before getting started, make sure you have the following tools installed:

- [.NET SDK 6](https://dotnet.microsoft.com/download)
- [SQLite](https://www.nuget.org/packages/sqlite-net)

## Configuration

### Database

To configure the database:

1. Ensure you have a PostgreSQL or SQLite database up and running.
2. Set the connection string in `appsettings.json` or `appsettings.{env}.json` based on the environment.

```json
{
  "SQLiteConnection": {
    "SQLiteConnection": "YourConnectionStringHere"
  }
}
```
3. first migration of database initialization is already present, just update your database to initialize it if you want to do it in codefirst

### Environment

The environment can be configured in appsettings.json and appsettings.[ASPNETCORE_ENVIRONMENT].json where ASPNETCORE_ENVIRONMENT can be Development, Staging or Production.

### Installation and Running

To run the project locally:

1. Clone this repository to your machine.
2. Set up the database as described above.
3. Open a console and navigate to the project's root directory.
4. Run the following command to start the application:

```bash
dotnet run
```

5.The application will start on https://localhost:7278 (by default). You can access the API documentation via Swagger at https://localhost:7278/swagger.


## Features

1. User management (registration, authentication)
2. Task management (creation, updating, deletion)
3. JWT authentication for secure resource access
4. Pagination, sorting, and filtering of tasks
E. xception handling and logging

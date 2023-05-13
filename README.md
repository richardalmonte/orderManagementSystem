# PhotoSì - Minimal Order Management System

## Description

This project is a minimal order management system built with .NET Core (C#) and Clean Architecture.
The system consists of several microservices that handle orders, products, users, and addresses.

The system was created as part of a software engineering interview.

## Getting Started

Instructions on how to clone the repository, run the project, and any other necessary steps.

## Technologies

- .NET Core
- Entity Framework Core
- xUnit
- AutoFixture
- Moq
- more...

## Installation

- Clone the repository: `git clone https://github.com/richardalmonte/orderManagementSystem.git`
- Navigate to the project directory: `cd orderManagementSystem`
- Open the solution in your IDE of choice (Visual Studio, Rider, vscode, etc.)
- Restore the NuGet packages: `dotnet restore`
- Build the solution: `dotnet build`
- Run the unit tests: `dotnet test`
- Run the migrations: `dotnet ef database update --project {projectName}`
- Run each microservice in a separate terminal window: `dotnet run --project {projectName}`
  

## Usage

Once the services are running, you can use a REST API testing tool to interact whit them.

## Architecture

The project is built with Clean Architecture in mind.
Which separates concerns into layers and enforces dependency rules. The layers are as follows:

- **Domain**: Contains the business logic and entities of the application.
- **Application**: Contains the application logic and interfaces for the application services.
- **Infrastructure**: Contains the implementation of the application services interfaces,
  such as data access and external services.

## API Documentation

The API documentation for each service can be found in their respective Swagger UI.

## Testing

The project follows a Test-Driven Development (TDD) approach, meaning that tests are written before the code.
Each service has unit tests that cover the business logic.

## Author

[Richard Almonte](https://github.com/richardalmonte)

## License

This project is licensed under the [MIT License](LICENSE).

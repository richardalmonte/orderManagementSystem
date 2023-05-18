# Minimal Order Management System

## Description

This project is a minimal order management system built with .NET Core (C#) and Clean Architecture.
The system consists of several microservices that handle orders, products, users, and addresses.

The system was created as part of a software engineering interview.

## Getting Started

- Clone the repository: `git clone https://github.com/richardalmonte/orderManagementSystem.git`
- Navigate to the project directory: `cd orderManagementSystem`
- Open the solution in your IDE of choice (Visual Studio, Rider, vscode, etc.).
- Restore the NuGet packages: `dotnet restore`
- Build the solution: `dotnet build`

## Technologies

- .NET Core
- Entity Framework Core
- Ocelot (API Gateway)
- xUnit
- AutoFixture
- Moq
- more...

## Running the Project

- Ensure each service and the API Gateway have unique port numbers specified.
- Update the `ocelot.json` configuration file in the API Gateway project, if necessary.
- Run the migrations: `dotnet ef database update --project {projectName}`
- Run the unit tests: `dotnet test`
- Start all the projects (You can set multiple startup projects in Visual Studio or run each microservice in a
  separate terminal window: `dotnet run --project {projectName}`).

## API Gateway

The API Gateway project is implemented using Ocelot, an API Gateway library for .NET Core. It allows for the aggregation
and routing of microservices through a single entry point. After starting all microservices, you can start the API
Gateway.
The configuration for the API Gateway is in the `ocelot.json` file, which maps the incoming Gateway requests to the
correct microservice.

The API Gateway listens at `https://localhost:5080`, and you can route to the microservices with the respective paths:

- `/gateway/products`
- `/gateway/orders`
- `/gateway/users`
- `/gateway/addresses`

Remember, always use the API Gateway's address for requests and not the individual microservice addresses.

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

### Unit Tests

Most services are covered by unit tests. To run these tests, navigate to the service's project directory and use
the `dotnet test` command.

For example, to run the tests for the `ProductService`, navigate to the `ProductService.Tests` directory and run:

```bash
dotnet test
```

This will run the tests and provide a summary of the results.

### API Testing

The repository includes .http files that allow testing of the APIs directly within your preferred IDE such as VSCode,
Visual Studio, or Rider.
These .http files contain pre-defined requests to different endpoints of the API and can be run directly from the
editor.

To use them, open the .http file in your editor, select a request, and use the editor's built-in HTTP client to send
the request. For example, in VSCode, you would click on "Send Request" above the definition of the request.

This way, you can test different endpoints and see the responses without having to leave your editor or use an external
tool.

## Future Improvements

This project, while a functioning example of a minimal order management system, was built within a
limited timeframe and certain constraints. Nonetheless, it showcases effective usage of microservices,
.NET, Entity Framework, and other technologies. To elevate this system further, there are several
enhancements and additional functionalities we could consider:

- **Docker Support**: Docker would help to containerize each service and its dependencies, ensuring that
  the application runs in the same environment across different platforms. It simplifies setup and
  deployment processes, especially in a microservices architecture.

- **Message Queuing**: Introducing a message queuing system like RabbitMQ or Amazon SQS would manage
  asynchronous communications more efficiently, beneficial when dealing with high volumes of requests or
  long-running tasks.

- **CQRS and Mediator**: Implementing the Command Query Responsibility Segregation (CQRS) pattern and Mediator pattern
  would separate read and write operations and centralize communication between classes, leading to more maintainable
  code and potentially better performance.

- **Domain-Driven Design (DDD)**: The use of DDD would align the software closely with the business requirements,
  leading to a more effective design.

- **Authentication/Authorization**: Incorporating authentication and authorization mechanisms would ensure that only
  authorized users can access certain endpoints of the system.

- **Advanced Logging and Monitoring**: A sophisticated logging system, possibly integrated with an external monitoring
  service, would offer more insights into the system's performance and allow for effective troubleshooting.

- **Caching**: A caching layer could boost the system's performance by reducing the number of direct database
  interactions.

- **Health Checks**: Health checks would permit real-time monitoring of the system's health and ensure optimal
  operation.

- **Additional Testing**: While the project is covered by unit tests, adding integration tests and end-to-end tests
  would increase reliability and robustness of the system.

- **Continuous Integration/Continuous Deployment (CI/CD)**: A CI/CD pipeline would automate testing, build, and
  deployment processes, promoting code quality and efficiency.

These enhancements, given more time and resources, would substantially boost the system's scalability, maintainability,
and overall performance.

## Author

[Richard Almonte](https://github.com/richardalmonte)

## License

This project is licensed under the [MIT License](LICENSE).

## References

- [.NET Core](https://dotnet.microsoft.com/download)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [Ocelot](https://ocelot.readthedocs.io/en/latest/)
- [xUnit](https://xunit.net/)
- [AutoFixture](https://github.com/AutoFixture/AutoFixture)
- [Moq](https://github.com/moq/moq4/wiki/Quickstart)
- [Docker](https://www.docker.com/)
- [RabbitMQ](https://www.rabbitmq.com/)
- [Amazon SQS](https://aws.amazon.com/sqs/)
- [CQRS](https://martinfowler.com/bliki/CQRS.html)
- [Mediator Pattern](https://refactoring.guru/design-patterns/mediator)
- [Domain-Driven Design (DDD)](https://martinfowler.com/tags/domain%20driven%20design.html)
- [Authentication/Authorization](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-5.0&tabs=visual-studio)
- [Advanced Logging and Monitoring](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-5.0)
- [Caching](https://docs.microsoft.com/en-us/aspnet/core/performance/caching/middleware?view=aspnetcore-5.0)
- [Health Checks](https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-5.0)
- [Additional Testing](https://docs.microsoft.com/en-us/dotnet/core/testing/)
- [Continuous Integration/Continuous Deployment (CI/CD)](https://www.visualstudio.com/learn/what-is-cicd/)

# Event Management System

A clean architecture-based event management system built with ASP.NET Core Web API.

Thanks to Windsurf!

## Technologies & Frameworks

- **.NET 9.0**
- **ASP.NET Core Web API**
- **Entity Framework Core 9.0**
- **SQL Server**
- **NUnit** for testing
- **Serilog** for logging
- **MediatR** for CQRS pattern
- **AutoMapper** for object mapping

## Architecture

The solution follows Clean Architecture (Onion Architecture) principles with the following layers:

1. **Core Layer**
   - Domain entities
   - Interfaces
   - Common utilities

2. **Application Layer**
   - Commands and Queries (CQRS)
   - DTOs
   - Interfaces
   - Validation
   - Business logic

3. **Infrastructure Layer**
   - Database context
   - Repositories
   - External service implementations
   - Logging implementation

4. **Presentation Layer (API)**
   - Controllers
   - Middleware
   - API configurations

## Features

- Event management (Create, Update, Delete, Cancel)
- Participant management
- CQRS pattern implementation
- Comprehensive error handling
- Database logging with Serilog
- Automatic database migrations
- Unit tests with NUnit

## Getting Started

1. **Prerequisites**
   - .NET 8.0 SDK
   - SQL Server
   - Visual Studio 2022 or VS Code

2. **Database Setup**
   ```bash
   dotnet ef database update
   ```

3. **Running the Application**
   ```bash
   dotnet run --project src/Presentation/EventManagement.API/EventManagement.API.csproj
   ```

4. **Running Tests**
   ```bash
   dotnet test Tests/EventManagement.Tests/EventManagement.Tests.csproj
   ```

## Project Structure

```
├── src/
│   ├── Core/                      # Domain entities and interfaces
│   ├── Application/              # Business logic and CQRS
│   ├── Infrastructure/           # Data access and external services
│   └── Presentation/            # API endpoints and configuration
└── tests/
    └── EventManagement.Tests/   # Unit tests
```

## Best Practices

- Clean Architecture principles
- SOLID principles
- CQRS pattern
- Repository pattern
- Unit testing
- Efficient EF Core queries
- Comprehensive error handling
- Proper logging implementation

## API Endpoints

### Events
- `POST /api/event` - Create a new event
- `PUT /api/event/{id}` - Update an event
- `DELETE /api/event/{id}` - Delete an event
- `PUT /api/event/{id}/cancel` - Cancel an event
- `POST /api/event/register-participant` - Register a participant

## Error Logging

The application uses Serilog for structured logging with the following sinks:
- Console logging
- File logging (daily rolling files)
- SQL Server logging (for errors)

## Testing

The solution includes comprehensive unit tests using:
- NUnit test framework
- Moq for mocking
- FluentAssertions for readable assertions

Tests cover:
- Commands and Queries
- Business logic
- API endpoints
- Error scenarios

## Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

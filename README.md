# Event Management System

A modern event management system built with .NET 8, following Clean Architecture principles.

## Features

- **Clean Architecture**: Organized in layers (Core, Application, Infrastructure, Presentation)
- **CQRS Pattern**: Using MediatR for command and query separation
- **Entity Framework Core**: Code-first approach with SQL Server
- **Error Handling**: Comprehensive error logging with Serilog and database tracking
- **Azure Key Vault**: Secure secret management
- **API Documentation**: Example requests in EventManagement.API.http
- **Automatic Database Migration**: Database and seed data created on startup
- **Unit Testing**: Comprehensive test coverage with NUnit

## Getting Started

1. Update connection string in `appsettings.json`
2. Configure Azure Key Vault URL if using
3. Run the application - database will be created automatically

## Project Structure

```
src/
├── Core/                  # Entities, interfaces, common
├── Application/          # Business logic, CQRS handlers
├── Infrastructure/       # Data access, external services
└── Presentation/        # API controllers, middleware
tests/
└── EventManagement.Tests/ # Unit tests
```

## API Endpoints

### Events

#### Create Event
- **Endpoint**: `POST /api/events`
- **Description**: Create a new event
- **Request Body**:
```json
{
    "name": "Event Name",
    "description": "Event Description",
    "startDate": "2024-03-01T10:00:00Z",
    "endDate": "2024-03-01T18:00:00Z",
    "maxParticipants": 100,
    "venueId": 1
}
```
- **Response**: Returns the created event ID

#### Get Event Details
- **Endpoint**: `GET /api/events/{id}`
- **Description**: Get detailed information about a specific event
- **Response**: Returns event details including participants

#### Update Event
- **Endpoint**: `PUT /api/events/{id}`
- **Description**: Update an existing event
- **Request Body**: Same as Create Event
- **Validation**:
  - Cannot update cancelled events
  - Cannot reduce max participants below current count

#### Delete Event
- **Endpoint**: `DELETE /api/events/{id}`
- **Description**: Delete an event
- **Validation**:
  - Cannot delete events with registered participants

#### List Events
- **Endpoint**: `GET /api/events`
- **Description**: Get a list of all events
- **Optional Query Parameters**:
  - `status`: Filter by event status
  - `from`: Filter by start date
  - `to`: Filter by end date

See `EventManagement.API.http` for example requests.

## Technologies

- ASP.NET Core 8
- Entity Framework Core
- MediatR
- FluentValidation
- Serilog
- Azure Key Vault
- AutoMapper
- NUnit (Testing)

## Best Practices

- SOLID Principles
- DRY (Don't Repeat Yourself)
- Repository Pattern
- Unit of Work Pattern
- Result Pattern for responses
- Comprehensive error logging
- Clean Architecture
- CQRS
- Test-Driven Development (TDD)

## Error Handling

All errors are:
1. Logged to the database using Serilog
2. Return appropriate HTTP status codes
3. Include meaningful error messages

Common error scenarios:
- 404: Resource not found
- 400: Invalid request (validation errors)
- 409: Business rule violations
- 500: Unexpected server errors

## Performance Considerations

- Efficient EF Core queries with proper includes
- Async/await for all I/O operations
- Proper indexing on database tables
- Response caching where appropriate

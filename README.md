# TaskManagement API

ASP.NET Core Web API for managing users, projects, and task items with Entity Framework Core and SQL Server.

## Tech Stack

- .NET 10 (`net10.0`)
- ASP.NET Core Web API
- Entity Framework Core (`Microsoft.EntityFrameworkCore.SqlServer`)
- OpenAPI support (`Microsoft.AspNetCore.OpenApi`)

## Project Structure

### Root

- `TaskManagement.api.slnx` — solution file.
- `TaskManagement.api/` — main Web API project.

### `TaskManagement.api/`

- `Program.cs` — app startup, dependency injection, CORS policy, OpenAPI setup, and middleware pipeline.
- `Controllers/` — REST endpoints:
  - `UsersController.cs` — register/login and CRUD for users.
  - `ProjectsController.cs` — CRUD and sorting for projects.
  - `TaskItemsController.cs` — CRUD and sorting for task items.
  - `WeatherForecastController.cs` — default sample controller.
- `Models/` — domain models, enums, DTOs, and EF Core entity configuration:
  - `User.cs`, `Project.cs`, `TaskItem.cs`
- `Data/`
  - `TaskDbContext.cs` — EF Core DbContext (`Users`, `Projects`, `TaskItems`).
- `Migrations/` — EF Core migrations and model snapshot.
- `appsettings.json` / `appsettings.Development.json` — configuration and connection strings.
- `TaskManagement.api.http` — sample HTTP request file.

## Core Domain

- **User**: identity, role (`Admin`, `Manager`, `Employee`), and ownership/assignment relations.
- **Project**: title, date range, status, and owner (`UserId`).
- **TaskItem**: task details, due date, status, priority, and links to project/user.

## API Routes (base: `/api`)

- `/Users`
  - `GET /Users`, `GET /Users/{id}`
  - `POST /Users/Register`
  - `POST /Users/Login`
  - `PUT /Users/{id}`, `DELETE /Users/{id}`
- `/Projects`
  - `GET /Projects`, `GET /Projects/{id}`
  - `POST /Projects`
  - `PUT /Projects/{id}`, `DELETE /Projects/{id}`
  - `GET /Projects/Sort?sortBy=username|startdate|status&order=asc|desc`
- `/TaskItems`
  - `GET /TaskItems`, `GET /TaskItems/{id}`
  - `POST /TaskItems`
  - `PUT /TaskItems/{id}`, `DELETE /TaskItems/{id}`
  - `GET /TaskItems/Sort?sortBy=user|createdate|due|status|priority&order=asc|desc`

## Run Locally

1. Update the `DefaultConnection` in `TaskManagement.api/appsettings.json` to your SQL Server instance.
2. Build and run:

```bash
dotnet build TaskManagement.api.slnx
dotnet run --project TaskManagement.api/TaskManagement.api.csproj
```

3. OpenAPI endpoint is available in development mode:
- `/openapi/v1.json`

## Notes

- Enum values are configured to serialize as strings in JSON responses.
- CORS policy `enableAll` currently allows any origin, method, and header.

# CarManagement API (Clean Architecture)

A production-ready .NET 8 API implementing Clean Architecture for managing car brands, lines (models), and model years. Provides REST and planned GraphQL endpoints, CQRS with MediatR, EF Core (SQL Server), caching (Redis + Memory), JWT auth, Serilog logging, API Versioning, rate limiting, and comprehensive tests.

## Solution Structure
```
CarManagement.sln
├── CarManagement.Domain/           # Core entities, repository contracts, specifications
├── CarManagement.Application/      # CQRS (MediatR), DTOs, validation, mapping, business rules
├── CarManagement.Infrastructure/   # EF Core DbContext, repositories, UoW, caching, JWT
├── CarManagement.API/              # Web API (REST v1, GraphQL), middlewares, DI
└── CarManagement.Tests/
    ├── CarManagement.UnitTests/
    └── CarManagement.IntegrationTests/
```

## Getting Started
- Prerequisites:
  - .NET SDK 8.x
  - SQL Server (LocalDB or full SQL Server)
  - Redis (optional for caching; fallback to MemoryCache is supported)

### Configure
- Connection string: `CarManagement.API/appsettings.json` key `ConnectionStrings:DefaultConnection`.
- Serilog sinks and logging level: `CarManagement.API/appsettings.json`.

### Run
```
dotnet build CarManagement.sln
 dotnet run --project CarManagement.API/CarManagement.API.csproj
```
- Run on a fixed port (helpful for bookmarks and `.http` files):
```
ASPNETCORE_URLS="http://localhost:5229" dotnet run --project CarManagement.API/CarManagement.API.csproj
```
- Provide a connection string via environment variable (macOS/Linux syntax):
```
export ConnectionStrings__DefaultConnection="Server=localhost,1433;Database=CarManagement;User Id=sa;Password=Your_strong_password123;TrustServerCertificate=True;"
dotnet run --project CarManagement.API/CarManagement.API.csproj
```
- If using HTTPS locally, trust dev certs once:
```
dotnet dev-certs https --trust
```
- Swagger UI: http://localhost:5000/swagger (port may vary)
- REST API base: `/api/v1`

### Migrations (to be added)
```
# Example
 dotnet ef migrations add Initial --project CarManagement.Infrastructure --startup-project CarManagement.API
 dotnet ef database update --project CarManagement.Infrastructure --startup-project CarManagement.API
```

## Features Implemented
- Domain entities: `Brand`, `Line`, `ModelYear` with soft-delete and timestamps.
- Specifications for querying with paging/sorting.
- Application CQRS: Brands CRUD, Lines by Brand.
- Infrastructure: EF Core with configurations and Unit of Work.
- API: v1 Brands endpoints, API versioning, response compression, CORS, rate limiting, Serilog.

## Swagger endpoint

<img width="1672" height="700" alt="image" src="https://github.com/user-attachments/assets/1c474cb0-8cf5-40a1-ad17-f76a6e6f8793" />

- http://localhost:5000/swagger 

### Endpoints - examples

#### Get all brands
- Method: `GET`
- URL: `/api/v1/brands`
- Example: http://localhost:5000/api/v1/brands

#### Get a brand by id
- Method: `GET`
- URL: `/api/v1/brands/{id}`
- Example: http://localhost:5000/api/v1/brands/1

#### Create a brand
- Method: `POST`
- URL: `/api/v1/brands`
- Example: http://localhost:5000/api/v1/brands




# Café Debug API — Copilot Instructions

## Project Overview
ASP.NET Core 9 REST API backend for the Café Debug podcast platform. Uses Clean Architecture to enable scalability and open-source contributions. Repository: https://github.com/JessicaNathany/cafedebug-backend.api

## Architecture
**Clean Architecture** with strict layering:
- **Domain** (`cafedebug-backend.domain`) — Entities, value objects, domain logic. No external dependencies.
- **Application** (`cafedebug.backend.application`) — Use cases, DTOs (records), mapping, validation. Depends only on Domain.
- **Infrastructure** (`cafedebug-backend.infrastructure`) — EF Core DbContext, repositories, external services (S3/MinIO, email). Implements Domain interfaces.
- **API** (`cafedebug-backend.api`) — Controllers, middleware, filters, configuration. Thin controllers delegate to Application services.
- **Tests** (`tests/cafedebug.backend.api.test`) — xUnit tests following AAA pattern.

**Key principle**: Domain has no external dependencies. Infrastructure implements domain interfaces. Controllers never directly touch Infrastructure.

## Tech Stack
- **.NET 9 (C# 13)** with nullable reference types enabled
- **Entity Framework Core 9** with MySQL via raw SQL when needed
- **xUnit + Moq + AutoFixture + FluentAssertions** for testing
- **AutoMapper** for DTO mapping
- **FluentValidation** for data validation
- **Serilog** for structured logging
- **JWT Bearer Token** for authentication
- **Docker + Docker Swarm** (production uses 3 replicas with rolling updates)
- **Caddy** as reverse proxy (TLS/HTTPS)

## Code Conventions & Standards
### Language & Naming
- **English only** throughout code, comments, and documentation.
- **PascalCase** for classes, methods, properties, interfaces (must start with `I`).
- **camelCase** for local variables and parameters.
- **Namespace pattern**: `cafedebug_backend.{layer}` (underscores, not hyphens).

### Modern C# Patterns
- **Primary Constructors** where applicable (reduces boilerplate in service/repository classes).
- **Collection expressions**: Use `[]` instead of `new List<T>()`, `[item1, item2]` instead of `new[] { item1, item2 }`.
- **File-scoped namespaces**: `namespace MyNamespace;` (no braces).
- **Records** for all DTOs (Request/Response), Value Objects, and immutable data structures.
- **Async/await** for all I/O operations. Method names must end in `Async`. Always accept `CancellationToken ct` parameter.

### Architecture Patterns
- **Controllers**: Keep extremely thin. Inject Application services; never create services inline.
- **Routing**: Use attribute routing (e.g., `[Route("api/v1/[controller]")]`).
- **Responses**: Return standard `IActionResult` types (`Ok()`, `NotFound()`, `BadRequest()`, `Created()`, etc.).
- **Validation**: Use FluentValidation validators in Application layer. Controllers trigger validation automatically.
- **Dependency Injection**: Constructor injection only. Register all services in `Program.cs` via extensions (e.g., `AddApplicationServices()`).
- **Entity Framework**: Use one DbContext per logical boundary. Never expose `DbSet<T>` or EF-specific types to Application layer.

### Testing Standards
- **AAA Pattern**: Arrange (setup), Act (execute), Assert (verify).
- **Test Naming**: `MethodName_StateUnderTest_ExpectedBehavior` (e.g., `Create_WithInvalidEmail_ReturnsBadRequest`).
- **Test Data**: Use AutoFixture fixtures for consistent data generation. Mock repositories with `MockQueryable.Moq`.
- **Coverage**: Code coverage expected; use `coverlet.collector` for metrics.

## Build & Test Commands
```bash
# Build
dotnet restore                                          # Restore NuGet packages
dotnet build                                            # Build entire solution (Debug)
dotnet build --configuration Release                    # Production build

# Test (run from repo root)
dotnet test                                             # Run all tests
dotnet test --configuration Release                     # Test in Release mode
dotnet test --filter "FullyQualifiedName~EpisodeTests"  # Run specific test class
dotnet test --filter "Name~Create_WithValidData"        # Run tests matching pattern
dotnet test /p:CollectCoverage=true                     # Generate coverage reports

# Database (requires local MySQL from debug-automation repo)
dotnet ef migrations add <MigrationName>                # Create migration
dotnet ef database update                               # Apply migrations
dotnet ef database update -1                            # Rollback one migration

# Run API locally (from repo root)
dotnet run --project src/cafedebug-backend.api          # Run on http://localhost:5000
dotnet run --project src/cafedebug-backend.api --environment Development  # Load appsettings.Development.json

# Docker (requires debug-automation repo for MySQL)
docker-compose up -d                                    # Start MySQL + dependencies
docker-compose down                                     # Stop containers
```

## Project Setup
1. **Clone** this repo and the [debug-automation](https://github.com/JessicaNathany/debug-automation) repo (provides MySQL setup script).
2. **Database**: Run `./cafedebug-setup.sh` from debug-automation repo to set up MySQL with test data.
3. **Configuration**: Copy `appsettings.json` → `appsettings.Development.json` and configure:
   - MySQL connection string
   - JWT secret key (in `.env` using `DotNetEnv`)
   - S3/MinIO credentials (if using storage)
4. **Restore & Build**: `dotnet restore && dotnet build`
5. **Run**: `dotnet run --project src/cafedebug-backend.api`

## Environment & Secrets
- Uses **DotNetEnv** to load `.env` file at startup.
- Configuration priority: `appsettings.{Environment}.json` > `appsettings.json` > environment variables.
- **Never commit secrets**. Use `.env` (git-ignored) or GitHub Secrets for CI/CD.
- User Secrets for local development: `dotnet user-secrets set "JwtSecret" "your-key"` (stored safely outside repo).

## CI/CD & Deployment
- **GitHub Actions** (`.github/workflows/ci-cd.yml`):
  - Builds on .NET 9 with caching
  - Runs full test suite with coverage (Cobertura format)
  - Pushes Docker image to GitHub Container Registry (GHCR) on main branch push
- **Production (AWS EC2)**:
  - Docker Swarm orchestration (3 replicas for high availability)
  - Caddy reverse proxy handles TLS/HTTPS
  - Rolling updates with zero-downtime deployment
  - ASP.NET Core health checks integrated into deployment pipeline

## Important Files & Entry Points
- **`src/cafedebug-backend.api/Program.cs`** — Application startup, DI registration, middleware configuration.
- **`src/cafedebug-backend.infrastructure/Data/CafedebugContext.cs`** — EF Core DbContext. All DB schema lives here.
- **`docs/CONTRIBUTING.md`** — Contribution guidelines and issue workflow.
- **`DEPLOYMENT.md`** — AWS EC2 deployment procedures.
- **`GEMINI.md`** — Detailed development conventions and architecture notes.
- **`Dockerfile`** — Multi-stage build for production image.
- **`docker-compose.yml` & `docker-compose.local.yml`** — Local dev environment.

## Development Workflow
- **Branching**: `feature/feature-name` or `fix/bug-name`.
- **Commits**: Follow semantic versioning logic. Include Co-authored-by trailer if pair programming.
- **PRs**: Include feature/bug description and list of changes.
- **Code Review**: PRs must pass CI (build + tests) and meet architecture standards.
- **Testing**: New features/fixes **must** include tests. Aim for meaningful coverage, not 100%.

## Quick Tips
- **Before starting work**: Pull latest `main`, run `dotnet restore` and `dotnet build` to ensure clean state.
- **Add dependencies carefully**: Avoid redundant packages. If using xUnit, don't add NSubstitute without justification.
- **Domain modeling**: Use rich domain models. Encapsulate business logic in entities/value objects, not just in services.
- **Logging**: Use Serilog structured logging. Log at appropriate levels (Debug/Info for normal flow, Warn/Error for issues).
- **Error handling**: Handle errors consistently. Never expose sensitive data in responses or logs.
- **Database changes**: Always create EF migrations, don't modify schema manually. Test migrations locally.
- **Postman**: Use `cafe-collection.json` at repo root. Create test user via `/auth/register` endpoint.
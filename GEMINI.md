# GEMINI.md

## Project Overview
**Café Debug API** is a .NET 9 Web API designed to serve as the backend for the Café Debug website. It follows **Clean Architecture** principles to decouple business logic from infrastructure and presentation, enabling scalability and open-source contributions.

### Tech Stack
- **Framework:** .NET 9 (C#)
- **Architecture:** Clean Architecture (Domain, Application, Infrastructure, API/Presentation)
- **Database:** MySQL (via Entity Framework Core)
- **Object Mapping:** AutoMapper
- **Validation:** FluentValidation
- **Authentication:** JWT Bearer Token
- **Logging:** Serilog
- **Testing:** xUnit, Moq
- **Infrastructure:** Docker, Docker Swarm, Caddy (Reverse Proxy), AWS S3/MinIO (Storage)

---

## Building and Running

### Prerequisites
- .NET 9 SDK
- MySQL Server (Local or via Docker)
- Docker (Optional, for local environment)

### Local Setup
1. **Database:** The project often relies on the [debug-automation](https://github.com/JessicaNathany/debug-automation) repository to spin up the local MySQL environment.
2. **Configuration:** 
   - Copy `appsettings.json` to `appsettings.Development.json`.
   - Ensure connection strings and JWT settings are correctly configured.
3. **Environment Variables:** The project uses `DotNetEnv` to load `.env` files.
4. **Commands:**
   - **Restore:** `dotnet restore`
   - **Build:** `dotnet build`
   - **Run API:** `dotnet run --project src/cafedebug-backend.api`
   - **Test:** `dotnet test`

---

## Development Conventions

### Coding Standards
- **English Only:** All code, comments, and documentation must be in English.
- **Modern C#:** Use **Primary Constructors** where applicable. Use collection expressions (e.g., `[]` instead of `new List<T>()`). Use file-scoped namespaces.
- **Async Operations:** Use async/await for I/O. Always append `Async` to asynchronous method names. Pass `CancellationToken` where applicable.
- **Controller-Based:** Use traditional Controllers; do **not** use Minimal APIs. Keep controllers thin; delegate logic to Application services.
- **DTOs:** Use C# **records** for Request and Response DTOs.
- **Domain:** Use rich domain models; encapsulate business logic within entities.
- **Naming:** PascalCase for classes/methods/properties, camelCase for local variables. Interfaces must start with `I`.
- **Documentation:** Avoid redundant comments; use them only for complex business rules.

### Project Structure
- `src/cafedebug-backend.domain`: Entities, value objects, and domain logic. No external dependencies.
- `src/cafedebug.backend.application`: Use cases, DTOs, interfaces, and mapping.
- `src/cafedebug-backend.infrastructure`: EF Core DbContext, repositories, and external service implementations (S3, Email). Implements Domain interfaces.
- `src/cafedebug-backend.api`: Controllers, middlewares, filters, and API configuration. Entry point.
- `tests/`: xUnit test projects mirroring the source structure.

### Workflow & Testing
- **Branching:** `feature/feature-name` or `fix/bug-name`.
- **Testing:** New features or bug fixes **must** include corresponding unit tests.
- **Test Pattern:** Strictly follow the **Arrange / Act / Assert (AAA)** pattern.
- **Test Naming:** Use `MethodName_StateUnderTest_ExpectedBehavior`.
- **Commits:** Follow semantic versioning logic for releases.

---

## Infrastructure and Deployment

### CI/CD
- **GitHub Actions:** Workflows are defined for CI (build/test) and CD (deployment to AWS).
- **Registry:** Docker images are pushed to GitHub Container Registry (GHCR).

### Production (AWS EC2)
- **Orchestration:** Docker Swarm.
- **Replicas:** Typically runs 3 replicas for high availability with rolling updates.
- **Reverse Proxy:** Caddy handles TLS/HTTPS and load balancing.
- **Health Checks:** Native ASP.NET Core health checks are integrated into the deployment pipeline for zero-downtime updates.

---

## Key Files
- `src/cafedebug-backend.api/Program.cs`: Application entry point and service registration.
- `src/cafedebug-backend.infrastructure/Data/CafedebugContext.cs`: EF Core database context.
- `docs/CONTRIBUTING.md`: Detailed contribution and development guidelines.
- `DEPLOYMENT.md`: Comprehensive guide for AWS EC2 deployment.

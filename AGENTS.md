# Repository Guidelines

## Project Structure & Module Organization
The solution root is `cafedebug-backend.api.sln`. Source code lives under `src/` and follows Clean Architecture:

- `src/cafedebug-backend.api`: ASP.NET Core entry point, controllers, filters, middleware, health checks.
- `src/cafedebug.backend.application`: use cases, DTOs, validators, mappings, service contracts.
- `src/cafedebug-backend.domain`: entities, errors, repository interfaces, shared domain primitives.
- `src/cafedebug-backend.infrastructure`: EF Core context, repositories, storage, security, external integrations.
- `tests/cafedebug.backend.api.test`: xUnit test suite, organized by feature and shared test helpers.

Keep dependencies flowing inward: API -> Application -> Domain, with Infrastructure implementing Domain/Application contracts.

## Build, Test, and Development Commands
- `dotnet restore`: restore NuGet packages for the solution.
- `dotnet build cafedebug-backend.api.sln --configuration Release`: compile all projects the same way CI does.
- `dotnet run --project src/cafedebug-backend.api/cafedebug-backend.api.csproj`: start the API locally.
- `dotnet test tests/cafedebug.backend.api.test/cafedebug.backend.api.test.csproj --configuration Release`: run the unit test project.
- `docker build -t cafedebug-backend.api .`: build the production image defined by `Dockerfile`.

## Coding Style & Naming Conventions
Use English for code, comments, and docs. Follow standard C# style: 4-space indentation, PascalCase for types/members, camelCase for locals/parameters, and `I`-prefixed interfaces. Keep controllers thin and avoid Minimal APIs. Prefer `record` types for request/response DTOs, async I/O methods ending in `Async`, and comments only for non-obvious business rules.

## Testing Guidelines
Tests use xUnit, Moq, AutoFixture, FluentAssertions, and `coverlet.collector`. Mirror the source area under `tests/` and keep tests in AAA form. Match the existing naming style, e.g. `UploadAsync_WhenUploadFails_ReturnFailedResult`. Add or update tests for every feature change and bug fix before opening a PR.

## Commit & Pull Request Guidelines
Recent history favors short, imperative commit subjects such as `Fix contributing link` or `adjust MaximumLength...`. Keep commits focused and descriptive. Use branches like `feature/episode-search` or `fix/banner-validation`. PRs should summarize the change, note affected layers, link the issue when applicable, and include test evidence (`dotnet test ...`). If an API contract changes, add request/response examples or Swagger proof.

## Configuration & Security Tips
`Program.cs` loads `.env` via `DotNetEnv`; keep secrets in `.env`, environment variables, or `dotnet user-secrets`. Never commit real credentials or populated `appsettings.Development.json`. Local MySQL setup is documented in `README.md` and `docs/CONTRIBUTING.md`.

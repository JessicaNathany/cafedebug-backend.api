# AGENTS.md

## Before acting
- Confirm the request against the current repository state before editing anything.
- Prefer the smallest change that satisfies the task; avoid speculative refactors.
- Respect Clean Architecture boundaries: dependencies flow `API -> Application -> Domain`, while Infrastructure implements Domain/Application contracts.
- Validate only to the depth the change requires; for docs-only changes, consistency review is enough.
- Use `README.md`, `docs/CONTRIBUTING.md`, and `DEPLOYMENT.md` as deeper references instead of duplicating long setup or deployment instructions.

## Repository purpose
- This repository contains the Café Debug backend API for the public website and admin area.
- Major feature areas called out in current docs include episodes, banners, team members, jobs, ads, debuggers, audience/content flows, authentication, and media upload.
- The backend is intentionally separate from the frontend so the site can evolve independently and expose controller-based HTTP endpoints.

## Architecture at a glance
- Solution file: `cafedebug-backend.api.sln`
- Runtime: ASP.NET Core on .NET 9.
- Architecture: Clean Architecture with feature-oriented organization inside each layer.
- Main layers:
  - `src/cafedebug-backend.api`: presentation layer, controllers, filters, middleware, health checks, configuration, `Program.cs`.
  - `src/cafedebug.backend.application`: use cases, DTOs, validators, mappings, service contracts, application services.
  - `src/cafedebug-backend.domain`: entities, domain errors, repository interfaces, shared primitives.
  - `src/cafedebug-backend.infrastructure`: EF Core/MySQL persistence, repositories, storage, security, external integrations.
  - `tests/cafedebug.backend.api.test`: xUnit test suite mirroring source areas with shared helpers/setups/verifications.
- Keep business rules out of controllers. Controllers should delegate to application services, and domain logic should stay in Domain/Application layers.

## Key entry points and reference files
- `src/cafedebug-backend.api/Program.cs`
  - Loads `.env` with `DotNetEnv`.
  - Registers Serilog, database/configuration services, Swagger, health checks, application/infrastructure services, and controller filters.
  - Uses controller mapping, HTTPS redirection, custom `AuthenticationMiddleware`, then `UseAuthentication()` / `UseAuthorization()`.
- `src/cafedebug-backend.api/Controllers/Admin/EpisodesController.cs`
  - Good example of the intended controller style: attribute-routed controller, thin actions, `IEpisodeService` delegation, `Authorize` on protected writes.
- `tests/cafedebug.backend.api.test/Application/Media/Services/ImageServiceTest.cs`
  - Representative AAA-style test file using xUnit, Moq, FluentAssertions, and helper classes.
- `tests/cafedebug.backend.api.test/cafedebug.backend.api.test.csproj`
  - Confirms active test stack: xUnit, Moq, AutoFixture, FluentAssertions, `coverlet.collector`.
- `README.md`
  - Canonical human-facing overview, local setup, feature overview, and endpoint summary.
- `docs/CONTRIBUTING.md`
  - Contributor expectations, feature domains, local environment notes.
- `DEPLOYMENT.md`
  - Production deployment reference for AWS EC2, Docker Swarm, Caddy, secrets, and health-check-based rollout behavior.

## Build, run, and test commands
- Restore: `dotnet restore`
- Build like CI: `dotnet build cafedebug-backend.api.sln --configuration Release`
- Run API locally: `dotnet run --project src/cafedebug-backend.api/cafedebug-backend.api.csproj`
- Run tests: `dotnet test tests/cafedebug.backend.api.test/cafedebug.backend.api.test.csproj --configuration Release`
- Build container: `docker build -t cafedebug-backend.api .`

## Implementation patterns to follow
- Use traditional controllers; do not introduce Minimal APIs.
- Keep controllers thin and application-focused: receive HTTP input, call an application service, return the result.
- Prefer `record` types for request/response DTOs.
- Use async I/O and suffix asynchronous methods with `Async`.
- Follow existing C# conventions: 4-space indentation, PascalCase for types/members, camelCase for locals/parameters, `I`-prefixed interfaces.
- Use English for code, comments, docs, and commit/PR text.
- Avoid comments except for non-obvious business rules; do not add noise comments.
- Prefer simple, explicit solutions over clever abstractions or extra libraries.
- Keep dependency direction intact; do not make Domain depend on Infrastructure or API.

## Testing expectations
- For behavior changes, add or update tests in `tests/cafedebug.backend.api.test` mirroring the source area.
- Follow Arrange / Act / Assert structure consistently.
- Match current naming patterns such as `UploadAsync_WhenUploadFails_ReturnFailedResult`.
- Reuse the existing testing stack and helpers before introducing new test utilities.
- For documentation-only changes, automated tests are not required; perform a consistency pass instead.

## Configuration and security guardrails
- `Program.cs` loads local settings from `.env`; prefer `.env`, environment variables, or `dotnet user-secrets` for secrets.
- Never commit real credentials or populated local secret files such as `appsettings.Development.json`.
- Local database/setup details live in `README.md` and `docs/CONTRIBUTING.md`; do not rewrite them in full here.
- The repo uses MySQL locally and in infrastructure references; storage integrations may target AWS S3 or MinIO depending on environment.
- Be careful not to expose sensitive values in code, fixtures, logs, screenshots, or documentation.

## Validation and delivery expectations for agents
- Prefer small, reviewable diffs.
- If changing API behavior or contracts, mention affected controllers/services and include validation evidence.
- If changing production/deployment behavior, cross-check `Dockerfile`, `docker-compose*.yml`, `Caddyfile`, and `DEPLOYMENT.md`.
- For PRs, keep commit subjects short and imperative, and include test evidence when code behavior changes.

## Multi-agent note
- Treat this file as the canonical shared agent context for Junie, Codex, and Copilot.
- If another agent-specific instruction file exists, it should point back here instead of duplicating guidance.

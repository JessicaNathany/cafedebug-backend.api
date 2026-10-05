# Workflow State: AWS S3 Public Image Storage

## State

Ready for review.

## Selected skills

- `build` for the approved Café Debug implementation workflow.
- `context7-mcp` to check the current AWS SDK reference before changing S3 usage.

## Paths changed

- `src/cafedebug-backend.infrastructure/Storage/AwsS3Service.cs`
- `src/cafedebug-backend.infrastructure/Storage/StorageSettings.cs`
- `src/cafedebug-backend.infrastructure/Common/Extensions/ServiceCollectionExtensions.cs`
- `src/cafedebug-backend.api/Controllers/Admin/ImageController.cs`
- `tests/cafedebug.backend.api.test/Infrastructure/Storage/AwsS3ServiceTest.cs`
- `tests/cafedebug.backend.api.test/Api/Controllers/Admin/ImageControllerAuthorizationTest.cs`
- `tests/cafedebug.backend.api.test/cafedebug.backend.api.test.csproj`
- `DEPLOYMENT.md`, `README.md`, `docs/CONTRIBUTING.md`, and `docs/CONTRIBUTING-pt-BR.md`

## Risks to verify

- Bucket-level public read requires an AWS-owner-approved policy and may be blocked by organization-level controls.
- Stored legacy URLs must continue to be deletable without a database migration.
- Railway credentials and configuration must remain secret and require a post-deploy authenticated smoke test.

## Acceptance evidence

- Focused S3 storage and authorization tests passed: 13 tests.
- `dotnet test cafedebug-backend.api.sln --configuration Release --no-restore` passed: 112 tests.
- `dotnet build cafedebug-backend.api.sln --configuration Release --no-restore` passed with zero warnings and errors.
- `git diff --check` passed.

## Remaining external validation

- An AWS owner must apply the documented IAM and bucket policies and create the dedicated access key.
- A Railway owner must set the documented secrets and configuration values, then complete the authenticated upload/public-read/delete smoke test after deployment.

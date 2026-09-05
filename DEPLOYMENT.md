# Production deployment

GitHub Actions validates changes. Railway builds the root Dockerfile and deploys successful commits from `JessicaNathany/cafedebug-backend.api`, branch `main`.

## Production service

| Setting | Value |
| --- | --- |
| Railway project | `cafedebug-backend.api-railway` |
| Environment | `production` |
| Service | `cafedebug-backend.api` |
| Source | `JessicaNathany/cafedebug-backend.api` |
| Branch / root directory | `main` / repository root |
| Builder / Dockerfile | Dockerfile / `/Dockerfile` |
| Automatic deployments | Enabled |
| Wait for CI | Enabled |
| Health check / timeout | `/health/ready` / 300 seconds |
| `PORT` / public target port | `8080` / `8080` |
| Deployment identity | Git commit SHA |

These are the required settings; verify them in Railway when configuring or auditing production. Workflow changes alone do not configure Railway or GitHub branch protection.

Keep existing database, storage, SMTP, JWT variables, domain, region, and replica configuration. Never copy secret values into this document or CI logs. The root container listens on port 8080 and runs as a non-root user. Its restore layer includes central package versions and the SDK configuration.

## CI and pull requests

`CI` runs on PRs targeting `main` and on pushes to `main`. Its single `Build & Test` job:

1. Uses the SDK from `global.json` and caches NuGet packages.
2. Restores the solution with direct and transitive dependency auditing. High/critical vulnerabilities (`NU1903`/`NU1904`) and audit retrieval failures (`NU1900`) fail CI. Lower severity vulnerabilities remain warnings.
3. Builds Release once, then runs the existing test project without rebuilding/restoring.
4. Collects coverage through `coverlet.collector` with `--collect:"XPlat Code Coverage"`.
5. Publishes a test/coverage summary and separate `test-results` (TRX) and `coverage-results` (Cobertura) artifacts, including after test failures.

There is no minimum coverage percentage. Missing or unreadable reports fail reporting explicitly. Reports cannot conceal a failing test run. Superseded PR runs are cancelled; main runs use distinct concurrency groups and are not cancelled by later commits. Jobs have a 20-minute timeout and read-only repository permissions.

No GitHub Actions job builds/publishes a Docker image, creates a draft release, or connects to EC2. Railway performs the production image build. GitHub releases are created manually for intentional version milestones.

## Protect main

A repository administrator must configure an **active** branch ruleset targeting `refs/heads/main`, with no routine bypass actors:

- Require a pull request and one approving review.
- Dismiss stale approvals when new commits are pushed.
- Require resolved review conversations.
- Require the `Build & Test` status check, with GitHub Actions as the expected source.
- Require the branch to be up to date with `main`.
- Block force pushes and branch deletion.
- Allow squash merging only; disable merge commits and rebase merges in repository settings.

Register `Build & Test` only after its first successful PR run, then activate the rules before merging. Do not require the retired `Build` or `Test` checks. Repository write access alone does not grant permission to change these settings.

The reviewed payload is [main-ruleset.json](docs/main-ruleset.json). After the first successful `Build & Test` PR run, an administrator can apply it with:

```bash
gh api repos/JessicaNathany/cafedebug-backend.api/rulesets --method POST \
  --input docs/main-ruleset.json
gh api repos/JessicaNathany/cafedebug-backend.api --method PATCH \
  -F allow_squash_merge=true -F allow_merge_commit=false -F allow_rebase_merge=false
```

Inspect existing rulesets first and update an equivalent rule instead of creating a duplicate. The status check source ID `15368` is GitHub Actions, verified from this repository's existing CI checks.

## Railway setup and rollout

1. Verify the service source and target branch in the table above. The Railway GitHub integration must have access to Jessica's repository and a project member must have contributor access.
2. Enable automatic deployments and **Wait for CI** in the service settings. The `push: main` workflow is required for this gate.
3. Set `PORT=8080`, health check `/health/ready`, and timeout 300 seconds. Preserve all unrelated settings. Setting changes should not launch an unvalidated deployment; apply them without redeploying while preparing the PR.
4. Verify the PR's `Build & Test` run, both artifacts, and summary. Activate branch protection before merge.
5. After an approved squash merge, verify the main push CI succeeds before Railway starts the deployment. Failed CI must prevent automatic deployment.
6. Verify the Railway deployment commit matches the merged main commit and readiness passes before traffic switches.
7. Check the public readiness endpoint without printing its response body:

   ```bash
   curl --fail --silent --show-error --output /dev/null \
     https://cafedebug-backendapi-production.up.railway.app/health/ready
   ```

Record the PR check/run URL, main CI URL, commit SHA, Railway deployment ID/status, and readiness HTTP result. A green PR alone is not evidence that the production rollout completed.

`/health/ready` checks database connectivity and an episode query. `/health/live` checks process liveness only. Railway health checks gate activation; they are not continuous uptime monitoring. Do not change the gate to liveness to hide a readiness failure. Inspect logs, configuration, and database availability instead.

## Rollback

Use the Railway service deployment history to roll back to a previously successful deployment. Record its commit SHA and deployment ID, verify readiness, and confirm the public endpoint. If the previous deployment is outside Railway's rollback retention, revert the faulty commit through a reviewed PR and let CI and autodeploy run normally.

Application rollback does not roll back database data or schema. The repository currently defines no EF migration implementation or startup migration execution. Do not add a `dotnet ef database update` pre-deploy command: the runtime image does not include an EF migration toolchain. Schema changes need a separately reviewed migration plan.

## Legacy infrastructure

[Historical EC2 deployment instructions](docs/legacy-ec2-deployment.md), `docker-compose.yml`, `Caddyfile`, and `scripts/deploy.sh` remain as legacy reference. `docker-compose.local.yml` remains available for local development. They are not used by Railway production.

Retiring the workflows does not delete EC2 resources, credentials, or existing Docker Hub/GHCR images. Decommissioning those resources is separate work.

## References

- [Railway GitHub autodeploy and Wait for CI](https://docs.railway.com/deployments/github-autodeploys)
- [Railway health checks](https://docs.railway.com/deployments/healthchecks)
- [Coverlet collector usage](https://github.com/coverlet-coverage/coverlet/blob/master/Documentation/VSTestIntegration.md)

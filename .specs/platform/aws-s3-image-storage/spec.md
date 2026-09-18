# AWS S3 Public Image Storage

## Status

Approved for implementation.

## Goal

Run the existing administrative image upload and deletion flow against the production AWS S3 bucket from Railway without changing the request or response contract, and keep previously stored S3 image URLs usable.

## Functional requirements

- `POST /api/v1/admin/images/upload` and `POST /api/v1/admin/images/delete` require the default API authentication policy.
- The application continues to use `IFileService`; no direct browser-to-S3 upload endpoint is introduced.
- New upload URLs are `{BaseUrl}/{key}`. Production `BaseUrl` is the bucket virtual-hosted S3 endpoint and local MinIO continues to use its configured endpoint.
- Uploads do not send public object ACLs. Public read access is owned by the bucket policy.
- Deletion accepts newly generated URLs and the legacy S3 URL forms for the configured bucket; database URLs are not migrated.
- Application startup rejects incomplete or invalid storage configuration without logging credentials.

## Non-goals

- Generic file and document uploads.
- Direct browser uploads, presigned URLs, CloudFront, S3 CORS, or a new S3 readiness health check.
- Creation or mutation of AWS and Railway resources by this repository.

## Acceptance criteria

- Anonymous requests to either image administration endpoint cannot upload or delete images.
- An authenticated upload writes to the configured bucket and returns the expected public URL.
- Delete derives the correct key from current and legacy URLs for the configured bucket.
- The S3 SDK request contains no public canned ACL.
- Local S3-compatible endpoint support remains available through `ServiceUrl`.
- The deployment guide specifies least-privilege IAM, bucket-policy, Railway-variable, rotation, smoke-test, and rollback steps without secret values.

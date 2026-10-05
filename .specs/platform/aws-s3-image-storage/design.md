# Design: AWS S3 Public Image Storage

## Architecture

The API layer remains responsible only for authenticated HTTP handling. `ImageService` keeps its existing application contract with `IFileService`. The Infrastructure `AwsS3Service` owns S3 client calls and URL/key translation.

`StorageSettings` remains bound from `Storage:AWS:S3`. Railway supplies the binding values and AWS standard credential variables; credentials are intentionally not bound into application settings or emitted in logs.

## Configuration

Production uses these Railway variables:

| Variable | Value |
| --- | --- |
| `AWS_ACCESS_KEY_ID` | Secret access-key identifier for the dedicated Railway IAM identity |
| `AWS_SECRET_ACCESS_KEY` | Secret key for that identity |
| `Storage__AWS__S3__Bucket` | Existing S3 bucket name |
| `Storage__AWS__S3__Region` | Bucket region |
| `Storage__AWS__S3__BaseUrl` | `https://{bucket}.s3.{region}.amazonaws.com` |
| `Storage__AWS__S3__ForcePathStyle` | `false` |
| `Storage__AWS__S3__UseHttp` | `false` |

`ServiceUrl` is unset in Railway. A configured `ServiceUrl` continues to select the local MinIO/S3-compatible client path.

## Object access

The existing bucket uses Bucket owner enforced ownership and ACLs remain disabled. The API identity receives only `s3:PutObject` and `s3:DeleteObject` for `images/*`, `episodes/*`, `banners/*`, `team-members/*`, and `contributors/*`. The bucket policy grants public `s3:GetObject` only to those same prefixes. The implementation does not send `PublicRead` or any other object ACL.

If account- or organization-level Block Public Access prevents that narrow bucket policy, the rollout stops for an AWS-owner decision; it must not weaken account-wide controls.

## Compatibility and failure handling

New images use the configured public base URL. URL-to-key extraction accepts that URL plus regional path-style and legacy virtual-hosted forms for the same configured bucket. Other hosts and malformed URLs are rejected before S3 deletion.

Configuration validation requires a bucket, absolute base URL, and a region when no custom service URL is configured. The existing Railway readiness check remains database-focused so it does not require broader S3 permissions.

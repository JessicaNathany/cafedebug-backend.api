using Microsoft.Extensions.Options;

namespace cafedebug_backend.infrastructure.Storage;

public class StorageSettings
{
    /// <summary>
    /// The S3 bucket name where files will be stored
    /// </summary>
    public string Bucket { get; set; } = string.Empty;
    
    /// <summary>
    /// The service URL (used for MinIO/LocalStack or custom endpoints)
    /// Leave empty for standard AWS S3
    /// </summary>
    public string? ServiceUrl { get; set; } = null;
    
    /// <summary>
    /// The base URL for accessing uploaded files
    /// For AWS S3: https://{bucket}.s3.{region}.amazonaws.com
    /// For MinIO: http://localhost:9000/{bucket}
    /// </summary>
    public string BaseUrl { get; set; } = string.Empty;
    
    /// <summary>
    /// AWS Region (e.g., "us-east-1", "sa-east-1")
    /// Required for AWS S3, not needed for MinIO
    /// </summary>
    public string? Region { get; set; } = null; 
    
    /// <summary>
    /// Use HTTP instead of HTTPS (only for local development)
    /// </summary>
    public bool UseHttp { get; set; } = false; 
    
    /// <summary>
    /// Force path-style URLs (required for MinIO/LocalStack)
    /// AWS S3 uses virtual-hosted style by default
    /// </summary>
    public bool ForcePathStyle { get; set; } = false;
}

public sealed class StorageSettingsValidator : IValidateOptions<StorageSettings>
{
    public ValidateOptionsResult Validate(string? name, StorageSettings settings)
    {
        var failures = new List<string>();
        var hasCustomServiceUrl = !string.IsNullOrWhiteSpace(settings.ServiceUrl);

        if (string.IsNullOrWhiteSpace(settings.Bucket))
            failures.Add("Storage:AWS:S3:Bucket must be configured.");

        if (!Uri.TryCreate(settings.BaseUrl, UriKind.Absolute, out var baseUrl) ||
            (baseUrl.Scheme != Uri.UriSchemeHttp && baseUrl.Scheme != Uri.UriSchemeHttps) ||
            !string.IsNullOrEmpty(baseUrl.Query) ||
            !string.IsNullOrEmpty(baseUrl.Fragment))
        {
            failures.Add("Storage:AWS:S3:BaseUrl must be an absolute HTTP(S) URL without a query or fragment.");
        }
        else if (!hasCustomServiceUrl && baseUrl.Scheme != Uri.UriSchemeHttps)
        {
            failures.Add("Storage:AWS:S3:BaseUrl must use HTTPS for AWS S3.");
        }

        if (hasCustomServiceUrl &&
            (!Uri.TryCreate(settings.ServiceUrl, UriKind.Absolute, out var serviceUrl) ||
             (serviceUrl.Scheme != Uri.UriSchemeHttp && serviceUrl.Scheme != Uri.UriSchemeHttps)))
        {
            failures.Add("Storage:AWS:S3:ServiceUrl must be an absolute HTTP(S) URL when configured.");
        }

        if (!hasCustomServiceUrl && string.IsNullOrWhiteSpace(settings.Region))
            failures.Add("Storage:AWS:S3:Region must be configured for AWS S3.");

        if (!hasCustomServiceUrl && settings.UseHttp)
            failures.Add("Storage:AWS:S3:UseHttp is only supported with a custom S3-compatible ServiceUrl.");

        return failures.Count == 0
            ? ValidateOptionsResult.Success
            : ValidateOptionsResult.Fail(failures);
    }
}

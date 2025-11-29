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
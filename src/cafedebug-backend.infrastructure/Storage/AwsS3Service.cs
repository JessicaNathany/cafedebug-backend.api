using System.Text.RegularExpressions;
using Amazon.S3;
using Amazon.S3.Model;
using cafedebug_backend.domain.Media;
using cafedebug_backend.domain.Media.Services;
using Microsoft.Extensions.Logging;

namespace cafedebug_backend.infrastructure.Storage;

/// <summary>
/// Service responsible for managing file uploads to AWS S3.
/// </summary>
public partial class AwsS3Service(IAmazonS3 awsClient, StorageSettings settings, ILogger<AwsS3Service> logger) : IFileService
{
    private static readonly string[] ImagePrefixes = [
        "images/",
        "episodes/",
        "banners/",
        "team-members/",
        "contributors/"
    ];

    [GeneratedRegex(@"^data:image\/[a-z]+;base64,", RegexOptions.Compiled)]
    private static partial Regex Base64ImagePattern();

    public async Task<string?> UploadImageAsync(string base64Image, string fileName, ImageFolder? imageFolder)
    {
        try
        {
            var imageBytes = ConvertBase64ToBytes(base64Image);
            var fullKey = BuildFileKey(fileName, imageFolder);

            await UploadToS3Async(imageBytes, fullKey);

            var imageUrl = BuildImageUrl(fullKey);
            
            logger.LogInformation("Image uploaded successfully to S3. Key: {Key}", fullKey);
            
            return imageUrl;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to upload image to S3. FileName: {FileName}", fileName);
           return string.Empty;
        }
    }

    public async Task<bool> DeleteImageAsync(string imageUrl)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
            {
                logger.LogWarning("Attempted to delete image with null or empty URL");
                return false;
            }
            
            var key = ExtractKeyFromUrl(imageUrl);
            
            if (string.IsNullOrWhiteSpace(key))
            {
                logger.LogWarning("Failed to extract key from URL: {ImageUrl}", imageUrl);
                return false;
            }
            
            var deleted = await DeleteFromS3Async(key);
            
            if(!deleted)
                return false;
            
            logger.LogInformation("Image deleted successfully from S3. Key: {Key}", key);
            
            return true;
        }
        catch (AmazonS3Exception s3Ex) when (s3Ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            logger.LogWarning("Image not found in S3. Key extracted from URL: {ImageUrl}", imageUrl);
            return false;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete image from S3. URL: {ImageUrl}", imageUrl);
            return false;
        }
    }
    
    private static byte[] ConvertBase64ToBytes(string base64Image)
    {
        var base64Data = Base64ImagePattern().Replace(base64Image, string.Empty);
        return Convert.FromBase64String(base64Data);
    }

    private static string BuildFileKey(string fileName, ImageFolder? imageFolder)
    {
        var contextPath = GetContextPath(imageFolder);
        
        return $"{contextPath}/{fileName}";
    }
    
    private static string GetContextPath(ImageFolder? imageFolder)
    {
        if (imageFolder is null)
            return "images";
    
        return imageFolder.Value switch
        {
            ImageFolder.Episodes => "episodes",
            ImageFolder.Banners => "banners",
            ImageFolder.TeamMembers => "team-members",
            ImageFolder.Contributors => "contributors",
            _ => throw new ArgumentOutOfRangeException(nameof(imageFolder), imageFolder, "Unsupported image folder")
        };
    }

    private async Task UploadToS3Async(byte[] imageBytes, string key)
    {
        using var imageStream = new MemoryStream(imageBytes);

        var request = new PutObjectRequest
        {
            InputStream = imageStream,
            BucketName = settings.Bucket,
            Key = key
        };

        await awsClient.PutObjectAsync(request);
    }
    
    private async Task<bool> DeleteFromS3Async(string key)
    {
        var deleteRequest = new DeleteObjectRequest
        {
            BucketName = settings.Bucket,
            Key = key
        };

        var response = await awsClient.DeleteObjectAsync(deleteRequest);

        return response.HttpStatusCode == System.Net.HttpStatusCode.NoContent;
    }

    private string ExtractKeyFromUrl(string imageUrl)
    {
        if (!Uri.TryCreate(imageUrl, UriKind.Absolute, out var imageUri))
        {
            return string.Empty;
        }

        foreach (var baseUri in GetSupportedBaseUris())
        {
            if (TryExtractObjectKey(imageUri, baseUri, out var key))
                return key;
        }

        logger.LogWarning("Image URL does not match the configured S3 bucket. URL: {ImageUrl}", imageUrl);
        return string.Empty;
    }

    private string BuildImageUrl(string key)
    {
        var baseUrl = settings.BaseUrl.TrimEnd('/');
        return $"{baseUrl}/{key}";
    }

    private IEnumerable<Uri> GetSupportedBaseUris()
    {
        var configuredBaseUrl = settings.BaseUrl.TrimEnd('/');

        if (Uri.TryCreate(configuredBaseUrl, UriKind.Absolute, out var configuredBaseUri))
        {
            yield return configuredBaseUri;

            if (Uri.TryCreate($"{configuredBaseUrl}/{settings.Bucket}", UriKind.Absolute, out var legacyConfiguredBaseUri))
                yield return legacyConfiguredBaseUri;
        }

        if (!string.IsNullOrWhiteSpace(settings.ServiceUrl) || string.IsNullOrWhiteSpace(settings.Region))
            yield break;

        var bucket = settings.Bucket;
        var region = settings.Region;
        var legacyBaseUrls = new[]
        {
            $"https://{bucket}.s3.{region}.amazonaws.com",
            $"https://s3.{region}.amazonaws.com/{bucket}",
            $"https://{bucket}.s3.amazonaws.com",
            $"https://s3.amazonaws.com/{bucket}"
        };

        foreach (var legacyBaseUrl in legacyBaseUrls)
        {
            if (Uri.TryCreate(legacyBaseUrl, UriKind.Absolute, out var legacyBaseUri))
                yield return legacyBaseUri;

            if (Uri.TryCreate($"{legacyBaseUrl}/{bucket}", UriKind.Absolute, out var duplicatedBucketLegacyBaseUri))
                yield return duplicatedBucketLegacyBaseUri;
        }
    }

    private static bool TryExtractObjectKey(Uri imageUri, Uri baseUri, out string key)
    {
        key = string.Empty;

        if (!Uri.Compare(imageUri, baseUri, UriComponents.SchemeAndServer, UriFormat.Unescaped, StringComparison.OrdinalIgnoreCase).Equals(0))
            return false;

        var basePath = baseUri.AbsolutePath.TrimEnd('/');
        var objectPath = imageUri.AbsolutePath;
        var pathPrefix = string.IsNullOrEmpty(basePath) ? "/" : $"{basePath}/";

        if (!objectPath.StartsWith(pathPrefix, StringComparison.Ordinal))
            return false;

        var extractedKey = Uri.UnescapeDataString(objectPath[pathPrefix.Length..]);
        if (string.IsNullOrWhiteSpace(extractedKey) || !ImagePrefixes.Any(prefix => extractedKey.StartsWith(prefix, StringComparison.Ordinal)))
            return false;

        key = extractedKey;
        return true;
    }
}

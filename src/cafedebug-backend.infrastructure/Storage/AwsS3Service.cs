using System.Text.RegularExpressions;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using cafedebug_backend.domain.Media;
using cafedebug_backend.domain.Media.Services;
using Microsoft.Extensions.Logging;

namespace cafedebug_backend.infrastructure.Storage;

/// <summary>
/// Service responsible for managing file uploads to AWS S3.
/// </summary>
public partial class AwsS3Service(IAmazonS3 awsClient,  StorageSettings settings, ILogger<AwsS3Service> logger) : IFileService
{
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
        
        var transferUtility = new TransferUtility(awsClient);

        var request = new TransferUtilityUploadRequest
        {
            InputStream = imageStream,
            BucketName = settings.Bucket,
            Key = key,
            CannedACL = S3CannedACL.PublicRead // Make images publicly accessible
        };

        await transferUtility.UploadAsync(request);
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

    /// <summary>
    /// Removes the base URL from the provided image URL to get the key.
    /// </summary>
    /// <remarks>
    /// Example: https://cafedebug-uploads.s3.amazonaws.com/episodes/20250115/image.jpg
    /// Result: episodes/20250115/image.jpg
    /// </remarks>
    /// <param name="imageUrl">The image URL to extract the key from.</param>
    /// <returns>The extracted key from the image URL.</returns>
    private string ExtractKeyFromUrl(string imageUrl)
    {
        var baseUrl = settings.BaseUrl.TrimEnd('/');
        
        if (!imageUrl.StartsWith(baseUrl, StringComparison.OrdinalIgnoreCase))
        {
            logger.LogWarning("Image URL does not match configured base URL. URL: {ImageUrl}, BaseUrl: {BaseUrl}", imageUrl, baseUrl);
            return string.Empty;
        }

        var key = imageUrl[baseUrl.Length..].TrimStart('/');
        return key;
    }

    private string BuildImageUrl(string key)
    {
        var baseUrl = settings.BaseUrl.TrimEnd('/');
        return $"{baseUrl}/{settings.Bucket}/{key}";
    }
}

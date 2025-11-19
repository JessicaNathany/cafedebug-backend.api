using cafedebug_backend.domain.Media.Errors;
using cafedebug_backend.domain.Media.Services;
using cafedebug_backend.domain.Shared;
using cafedebug.backend.application.Media.DTOs.Requests;
using cafedebug.backend.application.Media.DTOs.Responses;
using cafedebug.backend.application.Media.Interfaces;

namespace cafedebug.backend.application.Media.Services;

/// <summary>
/// Manage image uploads and deletions.
/// </summary>
public class ImageService(IFileService fileService) : IImageService
{
    public async Task<Result<ImageResponse>> UploadAsync(UploadImageRequest request)
    {
        var uniqueFileName = GenerateUniqueFileName(request.FileName);
        
        var imageUrl = await fileService.UploadImageAsync(request.Base64, uniqueFileName, request.ImageFolder);

        if (imageUrl is null || imageUrl.Length == 0)
            return Result.Failure<ImageResponse>(ImageError.FailureToUploadImage);
        
        var response = new ImageResponse(imageUrl);
        
        return Result.Success(response);
    }

    public async Task<Result> DeleteAsync(DeleteImageRequest request)
    {
        var deleted = await fileService.DeleteImageAsync(request.ImageUrl);
        
        return deleted ? Result.Success() : Result.Failure(ImageError.FailureToDeleteImage);
    }
    
    private static string GenerateUniqueFileName(string originalFileName)
    {
        var extension = Path.GetExtension(originalFileName);
        var nameWithoutExtension = Path.GetFileNameWithoutExtension(originalFileName);
        var sanitizedName = SanitizeFileName(nameWithoutExtension);
        
        return $"{sanitizedName}-{Guid.NewGuid():N}{extension}";
    }

    private static string SanitizeFileName(string fileName)
    {
        var invalidChars = Path.GetInvalidFileNameChars();
        var sanitized = string.Join("_", fileName.Split(invalidChars, StringSplitOptions.RemoveEmptyEntries));
        sanitized = sanitized.Replace(" ", "-").ToLowerInvariant();
        
        return sanitized.Length > 50 ? sanitized[..50] : sanitized;
    }
}
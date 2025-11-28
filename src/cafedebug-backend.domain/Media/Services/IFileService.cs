namespace cafedebug_backend.domain.Media.Services;

public interface IFileService
{
    Task<string?> UploadImageAsync(string base64Image, string fileName, ImageFolder? imageFolder);
    Task<bool> DeleteImageAsync(string imageUrl);
}
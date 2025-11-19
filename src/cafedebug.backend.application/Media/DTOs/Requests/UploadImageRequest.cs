using cafedebug_backend.domain.Media;

namespace cafedebug.backend.application.Media.DTOs.Requests;

public record UploadImageRequest(string Base64, string FileName, ImageFolder? ImageFolder);
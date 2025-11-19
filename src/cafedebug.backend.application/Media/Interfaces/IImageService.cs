using cafedebug_backend.domain.Shared;
using cafedebug.backend.application.Media.DTOs.Requests;
using cafedebug.backend.application.Media.DTOs.Responses;

namespace cafedebug.backend.application.Media.Interfaces;

public interface IImageService
{
    Task<Result<ImageResponse>> UploadAsync(UploadImageRequest uploadImageRequest);
    Task<Result> DeleteAsync(DeleteImageRequest request);
}
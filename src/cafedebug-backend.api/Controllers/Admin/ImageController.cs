using cafedebug_backend.domain.Shared;
using cafedebug.backend.application.Media.DTOs.Requests;
using cafedebug.backend.application.Media.DTOs.Responses;
using cafedebug.backend.application.Media.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace cafedebug_backend.api.Controllers.Admin;

[ApiController]
[Produces("application/json")]
[Route("api/v1/admin/images")]
[Tags("Admin - Images")]
public class ImageController(IImageService imageService) : ControllerBase
{
    [HttpPost("upload")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Result<ImageResponse>>> Upload([FromBody] UploadImageRequest request)
    {
        return await imageService.UploadAsync(request);
    }

    [HttpPost("delete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Result>> Delete([FromBody] DeleteImageRequest request)
    {
        return await imageService.DeleteAsync(request);
    }
}
using cafedebug_backend.domain.Shared;
using cafedebug.backend.application.Media.DTOs.Requests;
using cafedebug.backend.application.Media.DTOs.Responses;
using cafedebug.backend.application.Media.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cafedebug_backend.api.Controllers.Admin;

[ApiController]
[Produces("application/json")]
[Route("api/v1/admin/images")]
[Tags("Admin - Images")]
public class ImageController(IImageService imageService) : ControllerBase
{
    [HttpPost("upload")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<Result<ImageResponse>>> Upload([FromBody] UploadImageRequest request)
    {
        return await imageService.UploadAsync(request);
    }

    [HttpPost("delete")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<Result>> Delete([FromBody] DeleteImageRequest request)
    {
        return await imageService.DeleteAsync(request);
    }
}

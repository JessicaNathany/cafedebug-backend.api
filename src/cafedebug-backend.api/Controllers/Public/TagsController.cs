using cafedebug.backend.application.Banners.DTOs.Responses;
using cafedebug.backend.application.Common.Pagination;
using cafedebug.backend.application.Podcasts.Interfaces.Tags;
using cafedebug_backend.domain.Shared;
using Microsoft.AspNetCore.Mvc;

namespace cafedebug_backend.api.Controllers.Public
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/v1/public/tags")]
    [Tags("Public - Tags")]
    public class TagsController(ITagsService tagsService) : Controller
    {
        [HttpGet]
        [ProducesResponseType(typeof(BannerResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Result>> GetAllAsync([FromQuery] PageRequest request)
        {
            return await tagsService.GetAllAsync(request);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BannerResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result>> GetByIdAsync(int id)
        {
            return await tagsService.GetByIdAsync(id);
        }
    }
}

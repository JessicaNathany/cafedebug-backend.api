using cafedebug.backend.application.Banners.DTOs.Responses;
using cafedebug.backend.application.Banners.Interfaces;
using cafedebug.backend.application.Common.Pagination;
using cafedebug_backend.domain.Shared;
using Microsoft.AspNetCore.Mvc;

namespace cafedebug_backend.api.Controllers.Public
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/v1/public/banners")]
    [Tags("Public - Banners")]
    public class BannersController(IBannerService bannerService) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(BannerResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Result>> GetAllAsync([FromQuery] PageRequest request)
        {
            return await bannerService.GetAllAsync(request);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BannerResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result>> GetByIdAsync(int id)
        {
            return await bannerService.GetByIdAsync(id);
        }

        [HttpGet("{bannerName}")]
        [ProducesResponseType(typeof(BannerResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result>> GetByNameAsync(string bannerName)
        {
            return await bannerService.GetByNameAsync(bannerName);
        }
    }
}

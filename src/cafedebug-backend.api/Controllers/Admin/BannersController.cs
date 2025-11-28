using cafedebug.backend.application.Banners.DTOs.Requests;
using cafedebug.backend.application.Banners.DTOs.Responses;
using cafedebug.backend.application.Banners.Interfaces;
using cafedebug.backend.application.Common.Pagination;
using cafedebug_backend.domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cafedebug_backend.api.Controllers.Admin
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/v1/admin/banners")]
    [Tags("Admin - Banners")]
    public class BannersController(IBannerService bannerService) : ControllerBase
    {
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result>> CreateAsync([FromBody] BannerRequest request)
        {
            // TODO: need to implement token validation. I'll do that next part
            return await bannerService.CreateAsync(request);
        }

        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(BannerResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result>> UpdateAsync([FromBody] BannerRequest request, int id)
        {
            // TODO: need to implement token validation. I'll do that next part
            return await bannerService.UpdateAsync(request, id);
        }

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

        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(BannerResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result>> Delete(int id)
        {
            // TODO: need to implement token validation. I'll do that next part
            return await bannerService.DeleteAsync(id);
        }
    }
}

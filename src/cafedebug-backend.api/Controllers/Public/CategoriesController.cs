using cafedebug.backend.application.Banners.DTOs.Responses;
using cafedebug.backend.application.Common.Pagination;
using cafedebug.backend.application.Podcasts.Interfaces.Categories;
using cafedebug_backend.domain.Shared;
using Microsoft.AspNetCore.Mvc;

namespace cafedebug_backend.api.Controllers.Public
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/v1/public/categories")]
    [Tags("Public - Categories")]
    public class CategoriesController(ICategoryService categoriyService) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(BannerResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Result>> GetAllAsync([FromQuery] PageRequest request)
        {
            return await categoriyService.GetAllAsync(request);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BannerResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result>> GetByIdAsync(int id)
        {
            return await categoriyService.GetByIdAsync(id);
        }

        [HttpGet("{categoryName}")]
        [ProducesResponseType(typeof(BannerResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result>> GetByNameAsync(string categoryName)
        {
            return await categoriyService.GetByNameAsync(categoryName);
        }
    }
}

using cafedebug.backend.application.Common.Pagination;
using cafedebug.backend.application.Podcasts.Interfaces.Episodes;
using cafedebug_backend.domain.Shared;
using Microsoft.AspNetCore.Mvc;

namespace cafedebug_backend.api.Controllers.Public
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/v1/public/episodes")]
    [Tags("Public - Episodes")]
    public class EpisodesController(IEpisodeService episodeService) : ControllerBase
    {
        [HttpGet]
        [Route("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Result>> Get(int id)
        {
            return await episodeService.GetByIdAsync(id);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Result>> GetAll([FromQuery] PageRequest request)
        {
            return await episodeService.GetAllAsync(request);
        }
    }
}

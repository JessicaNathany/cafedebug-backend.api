using cafedebug.backend.application.Common.Pagination;
using cafedebug.backend.application.Podcasts.DTOs.Requests;
using cafedebug.backend.application.Podcasts.DTOs.Responses;
using cafedebug.backend.application.Podcasts.Interfaces.Teams;
using cafedebug_backend.domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cafedebug_backend.api.Controllers.Admin
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/v1/admin/teams")]
    [Tags("Admin - Teams")]
    public class TeamsController(ITeamService teamService) : ControllerBase
    {
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(TeamResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result>> CreateAsync([FromBody] TeamRequest request)
        {
            return await teamService.CreateAsync(request);
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize]
        [ProducesResponseType(typeof(TeamResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result>> UpdateAsync(int id, [FromBody] TeamRequest request)
        {
            return await teamService.UpdateAsync(id, request);
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<TeamResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Result>> GetAllAsync([FromQuery] PageRequest request)
        {
            return await teamService.GetAllAsync(request);
        }

        [HttpGet]
        [Route("{id:int}")]
        [ProducesResponseType(typeof(TeamResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result>> GetByIdAsync(int id)
        {
            return await teamService.GetByIdAsync(id);
        }

        [HttpGet]
        [Route("by-name/{name}")]
        [ProducesResponseType(typeof(TeamResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result>> GetByNameAsync(string name)
        {
            return await teamService.GetByNameAsync(name);
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result>> Delete(int id)
        {
            return await teamService.DeleteAsync(id);
        }
    }
}

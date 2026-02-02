using cafedebug.backend.application.Common.Pagination;
using cafedebug.backend.application.Podcasts.Interfaces.Teams;
using cafedebug_backend.domain.Shared;
using cafedebug.backend.application.Podcasts.DTOs.Requests;
using cafedebug.backend.application.Podcasts.DTOs.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cafedebug_backend.api.Controllers.Admin
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/v1/admin/teams")]
    [Tags("Admin - Teams")]
    public class TeamsController(ITeamMemberService teamMemberService) : ControllerBase
    {
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(TeamMemberResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result>> CreateAsync([FromBody] TeamMemberRequest request)
        {
            return await teamMemberService.CreateAsync(request);
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize]
        [ProducesResponseType(typeof(TeamMemberResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result>> UpdateAsync(int id, [FromBody] TeamMemberRequest request)
        {
            return await teamMemberService.UpdateAsync(id, request);
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<TeamMemberResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Result>> GetAllAsync([FromQuery] PageRequest request)
        {
            return await teamMemberService.GetAllAsync(request);
        }

        [HttpGet]
        [Route("{id:int}")]
        [ProducesResponseType(typeof(TeamMemberResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result>> GetByIdAsync(int id)
        {
            return await teamMemberService.GetByIdAsync(id);
        }
        

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result>> Delete(int id)
        {
            return await teamMemberService.DeleteAsync(id);
        }
    }
}

using cafedebug.backend.application.Accounts.Interfaces;
using cafedebug.backend.application.Accounts.DTOs.Requests;
using cafedebug.backend.application.Accounts.DTOs.Response;
using Microsoft.AspNetCore.Mvc;
using cafedebug_backend.domain.Shared;
using Microsoft.AspNetCore.Authorization;
using cafedebug.backend.application.Common.Pagination;

namespace cafedebug_backend.api.Controllers.Admin;
[ApiController]
[Produces("application/json")]
[Route("api/v1/admin/users")]
[Tags("Admin - Users")]
public class UsersController(IUserService userService)  : ControllerBase
{
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(UserAdminResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<Result>> CreateAsync([FromBody] UserAdminRequest request)
    {
        return await userService.CreateAsync(request);
    }

    [HttpPut("{id:int}")]
    [Authorize]
    [ProducesResponseType(typeof(UserAdminResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Result>> UpdateAsync([FromBody] UserAdminRequest request, int id)
    {
        return await userService.UpdateAsync(request, id);
    }

    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(PagedResult<UserAdminResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Result>> GetAllAsync([FromQuery] PageRequest request)
    {
        return await userService.GetAllAsync(request);
    }

    [HttpGet("{id:int}")]
    [Authorize]
    [ProducesResponseType(typeof(UserAdminResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<Result>> GetByIdAsync(int id)
    {
        return await userService.GetByIdAsync(id);
    }

    [HttpDelete("{id:int}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<Result>> Delete(int id)
    {
        return await userService.DeleteAsync(id);
    }
}

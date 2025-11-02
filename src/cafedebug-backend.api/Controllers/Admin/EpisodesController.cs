using cafedebug_backend.domain.Shared;
using cafedebug.backend.application.Common.Pagination;
using cafedebug.backend.application.Podcasts.DTOs.Requests;
using cafedebug.backend.application.Podcasts.Interfaces.Episodes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace cafedebug_backend.api.Controllers.Admin;

/// <summary>
/// Manages podcast episodes for administrators
/// </summary>
/// <remarks>
/// This controller provides CRUD operations for managing podcast episodes in the system.
/// All endpoints require administrative privileges.
/// </remarks>
[ApiController]
[Produces("application/json")]
[Route("api/v1/admin/episodes")]
[Tags("Admin - Episodes")]
public class EpisodesController(IEpisodeService episodeService) : ControllerBase
{
    /// <summary>
    /// Retrieves a specific podcast episode by its identifier.
    /// </summary>
    /// <remarks>
    /// Fetches detailed information about a podcast episode based on the provided unique identifier.
    /// </remarks>
    /// <param name="id">The unique identifier of the podcast episode to retrieve</param>
    /// <returns>The result containing the episode data if found</returns>
    /// <response code="200">Returns the requested episode details</response>
    /// <response code="404">If the episode with the specified identifier is not found</response>
    [HttpGet]
    [Route("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Result>> Get(int id)
    {
        return await episodeService.GetByIdAsync(id);
    }
    
    /// <summary>
    /// Creates a new episode
    /// </summary>
    /// <remarks>
    /// Creates a new podcast episode with the provided information.
    /// </remarks>
    /// <param name="request">The episode creation request data</param>
    /// <returns>The newly created episode</returns>
    /// <response code="201">Returns the newly created episode</response>
    /// <response code="400">If the request data is invalid or business rules are violated</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Result>> Create([FromBody] EpisodeRequest request)
    {
        return await episodeService.CreateAsync(request);
    }

    /// <summary>
    /// Updates an existing podcast episode identified by its unique identifier.
    /// </summary>
    /// <remarks>
    /// Performs an update operation on a specified podcast episode using the given data.
    /// </remarks>
    /// <param name="id">The unique identifier of the podcast episode to update</param>
    /// <param name="request">The updated data for the podcast episode</param>
    /// <returns>The result of the update operation</returns>
    /// <response code="200">Indicates that the podcast episode was successfully updated</response>
    /// <response code="400">If the request payload is invalid or contains validation errors</response>
    /// <response code="404">If a podcast episode with the specified identifier could not be found</response>
    [HttpPut]
    [Route("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Result>> Update(int id, [FromBody] EpisodeRequest request)
    {
        return await episodeService.UpdateAsync(id, request);
    }

    /// <summary>
    /// Deletes a specific podcast episode by its identifier.
    /// </summary>
    /// <remarks>
    /// Removes the podcast episode identified by the provided unique identifier from the system.
    /// Administrative privileges are required to perform this action.
    /// </remarks>
    /// <param name="id">The unique identifier of the podcast episode to delete</param>
    /// <returns>The result indicating the success or failure of the deletion operation</returns>
    /// <response code="200">The episode was deleted</response>
    /// <response code="400">The request was invalid</response>
    /// <response code="404">If the episode with the specified identifier is not found</response>
    /// <response code="401">If the user is unauthorized to perform this operation</response>
    /// <response code="403">If the user does not have the required administrative permissions</response>
    [HttpDelete]
    [Route("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Result>> Delete(int id)
    {
        return await episodeService.DeleteAsync(id);
    }

    /// <summary>
    /// Retrieves a paginated list of all podcast episodes.
    /// </summary>
    /// <remarks>
    /// Provides a collection of podcast episodes based on the pagination details provided in the request.
    /// Only accessible by users with administrative privileges.
    /// </remarks>
    /// <param name="request">The pagination details, including the page number and size.</param>
    /// <returns>A paginated result containing the list of podcast episodes.</returns>
    /// <response code="200">Returns the paginated list of episodes.</response>
    /// <response code="400">If the request contains invalid pagination details.</response>
    /// <response code="401">If the user is not authenticated.</response>
    /// <response code="403">If the user is unauthorized to access this resource.</response>
    /// <response code="404">If episodes could not be found for the specified request parameters.</response>
    /// <response code="500">If an unexpected server error occurs.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Result>> GetAll([FromQuery]PageRequest request)
    {
        return await episodeService.GetAllAsync(request);
    }
}
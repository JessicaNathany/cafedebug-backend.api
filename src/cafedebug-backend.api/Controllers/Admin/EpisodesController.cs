using cafedebug_backend.domain.Shared;
using cafedebug.backend.application.Podcasts.DTOs.Requests;
using cafedebug.backend.application.Podcasts.Interfaces;
using cafedebug.backend.application.Podcasts.Interfaces.Episodes;
using Microsoft.AspNetCore.Mvc;

namespace cafedebug_backend.api.Controllers.Admin;

[Route("api/v1/admin/episodes")]
public class EpisodesController(ICreateEpisodeService createEpisodeService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        throw new NotImplementedException();
    }
        
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Result>> Create([FromBody] CreateEpisodeRequest request)
    {
        return await createEpisodeService.Handle(request);
    }

    [Route("{id:int}")]
    [HttpPut]
    public async Task<ActionResult> Update(int id) 
    {
        throw new NotImplementedException();
    }

    [Route("{id:int}")]
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        throw new NotImplementedException();
    }
}
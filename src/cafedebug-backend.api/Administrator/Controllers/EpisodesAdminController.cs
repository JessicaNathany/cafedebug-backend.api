using cafedebug.backend.application.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace cafedebug_backend.api.Administrator.Controllers
{
    [Area(nameof(Administrator))]
    [Route("api/episodio-admin")]
    public class EpisodesAdminController : ControllerBase
    {

        [Route("search-episodes")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            throw new NotImplementedException();
        }

        [Route("new-episode")]
        [HttpPost]
        public async Task<IActionResult> Create(EpisodeViewModel model)
        {
            throw new NotImplementedException();
        }

        [Route("edit-episode/{id}")]
        [HttpPut]
        public async Task<ActionResult> Update(int id) 
        {
            throw new NotImplementedException();
        }

        [Route("delete-episode/{id}")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}

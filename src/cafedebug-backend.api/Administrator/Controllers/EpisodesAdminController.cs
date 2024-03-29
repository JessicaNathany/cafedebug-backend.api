using cafedebug.backend.application.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace cafedebug_backend.api.Administrator.Controllers
{
    [Area(nameof(Administrator))]
    [Route("api/episodio-admin")]
    public class EpisodesAdminController : ControllerBase
    {

        [Route("listar-episodios")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            throw new NotImplementedException();
        }

        [Route("novo-episodio")]
        [HttpPost]
        public async Task<IActionResult> Create(EpisodeViewModel model)
        {
            throw new NotImplementedException();
        }

        [Route("editar-episodio/{id}")]
        [HttpPut]
        public async Task<ActionResult> Update(int id) 
        {
            throw new NotImplementedException();
        }

        [Route("delete-episodio/{id}")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}

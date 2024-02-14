using cafedebug.backend.application.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace cafedebug_backend.api.Administrator.Controllers
{
    [Area(nameof(Administrator))]
    [Route("administrador/episodios")]
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

        [Route("editar-episódio/{code:guid}")]
        [HttpPut]
        public async Task<ActionResult> Update(Guid code)
        {
            throw new NotImplementedException();
        }

        [Route("delete-episódio")]
        [HttpPost]
        public async Task<IActionResult> Delete(Guid code)
        {
            throw new NotImplementedException();
        }
    }
}

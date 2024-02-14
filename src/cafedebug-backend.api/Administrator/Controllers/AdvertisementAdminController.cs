using cafedebug.backend.application.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cafedebug_backend.api.Administrator.Controllers
{
    public class AdvertisementsAdminController : ControllerBase
    {
        public AdvertisementsAdminController()
        {

        }


        [Route("listar-anuncios")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            throw new NotImplementedException();
        }

        [Route("listar-anuncios/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetById()
        {
            throw new NotImplementedException();
        }

        [Authorize]
        [Route("novo-anuncios")]
        [HttpPost]
        public async Task<IActionResult> Create(AdvertisementViewModel model)
        {
            throw new NotImplementedException();
        }

        [Authorize]
        [Route("editar-anuncios/{id}")]
        [HttpPut]
        public async Task<ActionResult> Update(Guid code)
        {
            throw new NotImplementedException();
        }

        [Authorize]
        [Route("delete-anuncios")]
        [HttpPost]
        public async Task<IActionResult> Delete(Guid code)
        {
            throw new NotImplementedException();
        }
    }
}

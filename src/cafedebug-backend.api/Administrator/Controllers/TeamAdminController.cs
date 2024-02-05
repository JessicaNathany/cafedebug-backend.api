using cafedebug.backend.application.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cafedebug_backend.api.Administrator.Controllers
{
    public class TeamAdminController : BaseAdminController
    {
        public TeamAdminController()
        {

        }
        public IActionResult Index()
        {
            return View();
        }

        [Route("listar-debugers")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            throw new NotImplementedException();
        }

        [Route("listar-debugers/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetById()
        {
            throw new NotImplementedException();
        }

        [Authorize]
        [Route("novo-debugers")]
        [HttpPost]
        public async Task<IActionResult> Create(TeamViewModel model)
        {
            throw new NotImplementedException();
        }

        [Authorize]
        [Route("editar-debuger/{id}")]
        [HttpPut]
        public async Task<ActionResult> Update(Guid code)
        {
            throw new NotImplementedException();
        }

        [Authorize]
        [Route("delete-debuger")]
        [HttpPost]
        public async Task<IActionResult> Delete(Guid code)
        {
            throw new NotImplementedException();
        }
    }
}

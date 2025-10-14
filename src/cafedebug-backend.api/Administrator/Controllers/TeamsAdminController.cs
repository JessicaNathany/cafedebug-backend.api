using cafedebug.backend.application.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cafedebug_backend.api.Administrator.Controllers
{
    [Area(nameof(Administrator))]
    [Route("api/teams-admin")]
    public class TeamsAdminController : ControllerBase
    {
        public TeamsAdminController()
        {

        }

        [Route("list-debugers")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            throw new NotImplementedException();
        }

        [Route("get-debuger/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetById()
        {
            throw new NotImplementedException();
        }

        [Authorize]
        [Route("new-debuger")]
        [HttpPost]
        public async Task<IActionResult> Create(TeamViewModel model)
        {
            throw new NotImplementedException();
        }

        [Authorize]
        [Route("edit-debuger/{id}")]
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

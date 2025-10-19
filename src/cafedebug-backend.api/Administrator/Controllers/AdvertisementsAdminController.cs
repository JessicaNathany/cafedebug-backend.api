using cafedebug.backend.application.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cafedebug_backend.api.Administrator.Controllers
{
    [ApiController]
    [Area(nameof(Administrator))]
    [Produces("application/json")]
    [Route("api/v1/admin-advertisements")]
    public class AdvertisementsAdminController : ControllerBase
    {
        public AdvertisementsAdminController()
        {

        }


        [Route("list-advertisements")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            throw new NotImplementedException();
        }

        [Route("get-advertisements/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetById()
        {
            throw new NotImplementedException();
        }

        [Authorize]
        [Route("new-advertisements")]
        [HttpPost]
        public async Task<IActionResult> Create(AdvertisementViewModel model)
        {
            throw new NotImplementedException();
        }

        [Authorize]
        [Route("edit-advertisements/{id}")]
        [HttpPut]
        public async Task<ActionResult> Update(Guid code)
        {
            throw new NotImplementedException();
        }

        [Authorize]
        [Route("delete-advertisements")]
        [HttpPost]
        public async Task<IActionResult> Delete(Guid code)
        {
            throw new NotImplementedException();
        }
    }
}

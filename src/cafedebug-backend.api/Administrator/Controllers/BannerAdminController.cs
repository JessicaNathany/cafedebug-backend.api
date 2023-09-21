using Microsoft.AspNetCore.Mvc;

namespace cafedebug_backend.api.Administrator.Controllers
{
    [Area(nameof(Administrator))]
    [Route("administrador/banners")]
    public class BannerAdminController : BaseAdminController
    {
        public IActionResult Index()
        {
            return View();
        }


    }
}

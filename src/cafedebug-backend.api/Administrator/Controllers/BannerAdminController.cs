using AutoMapper;
using cafedebug_backend.application.Request;
using cafedebug_backend.application.Response;
using cafedebug_backend.domain.Entities;
using cafedebug_backend.domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cafedebug_backend.api.Administrator.Controllers
{
    [Route("api/banner-admin")]
    [ApiController]
    public class BannerAdminController : BaseAdminController
    {
        private readonly ILogger<BannerAdminController> _logger;
        private readonly IBannerService _bannerService;
        private readonly IMapper _mapper;
        public BannerAdminController(IBannerService bannerService, ILogger<BannerAdminController> logger, IMapper mapper)
        {
            _bannerService = bannerService;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult GetAll(CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        [HttpGet]
        public ActionResult GetById(CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        [HttpPost]
        [Route("novo-banner")]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] BannerRequest bannerRequest, CancellationToken cancellationToken)
        {
            try
            {
                if (bannerRequest is null)
                    return BadRequest("BannerRequest can be not null.");

                var banner = _mapper.Map<Banner>(bannerRequest);

                var result = await _bannerService.CreateAsync(banner, cancellationToken);

                if (!result.IsSuccess)
                    return BadRequest(result.Error);

                var bannerResponse = _mapper.Map<BannerResponse>(result.Value);

                return Ok(bannerResponse);
            }
            catch (NullReferenceException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpDelete]
        public ActionResult Delete(int id, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        [HttpPost]
        public ActionResult Update([FromBody] BannerRequest bannerRequest, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}

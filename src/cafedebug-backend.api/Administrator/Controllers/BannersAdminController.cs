using AutoMapper;
using cafedebug.backend.application.Service;
using cafedebug_backend.application.Request;
using cafedebug_backend.application.Response;
using cafedebug_backend.domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cafedebug_backend.api.Administrator.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/v1/banner-admin")]
    public class BannersAdminController : ControllerBase
    {
        private readonly BannerService _bannerService;
        private readonly IMapper _mapper;
        public BannersAdminController(BannerService bannerService, IMapper mapper)
        {
            _bannerService = bannerService;
            _mapper = mapper;
        }

        [HttpPost]
        [Authorize]
        [Route("novo-banner")]
        [ProducesResponseType(typeof(BannerResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] BannerRequest bannerRequest)
        {
            try
            {
                // check auth token

                if (!ModelState.IsValid)
                    return BadRequest("BannerRequest can be not valid.");

                if (bannerRequest is null)
                    return BadRequest("BannerRequest can be not null.");

                var banner = _mapper.Map<Banner>(bannerRequest);

                var result = await _bannerService.CreateAsync(banner);

                if (!result.IsSuccess)
                    return BadRequest(result.Error);

                var bannerResponse = _mapper.Map<BannerResponse>(result.Value);

                return StatusCode(StatusCodes.Status201Created, bannerResponse);
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

        [HttpPut]
        [Authorize]
        [Route("editar-banner")]
        [ProducesResponseType(typeof(BannerResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromBody] BannerRequest bannerRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("BannerRequest can be not valid.");

                if (bannerRequest is null)
                    return BadRequest("BannerRequest can be not null.");

                var banner = _mapper.Map<Banner>(bannerRequest);

                var result = await _bannerService.UpdateAsync(banner);

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

        [HttpGet]
        [Route("banners")]
        [ProducesResponseType(typeof(BannerResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var banners = await _bannerService.GetAllAsync();

                if (!banners.IsSuccess)
                    return BadRequest(banners.Error);

                var bannersResponse = _mapper.Map<List<BannerResponse>>(banners.Value);

                return Ok(bannersResponse);
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

        [HttpGet]
        [Route("buscar-banner/{id}")]
        [ProducesResponseType(typeof(BannerResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var banner = await _bannerService.GetByIdAsync(id);

                if (!banner.IsSuccess)
                    return BadRequest(banner.Error);

                var bannerResponse = _mapper.Map<BannerResponse>(banner.Value);

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
        [Authorize]
        [Route("deletar-banner/{id}")]
        [ProducesResponseType(typeof(BannerResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("BannerRequest can be not valid.");

                if (id is 0)
                    return BadRequest("Banner Id can be not null.");

                var result = await _bannerService.DeleteAsync(id);

                if (!result.IsSuccess)
                    return BadRequest(result.Error);

                return Ok();
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
    }
}

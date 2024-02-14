using AutoMapper;
using cafedebug_backend.application.Request;
using cafedebug_backend.application.Response;
using cafedebug_backend.domain.Entities;
using cafedebug_backend.domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cafedebug_backend.api.Administrator.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/banner-admin")]
    public class BannersAdminController : ControllerBase
    {
        private readonly IBannerService _bannerService;
        private readonly IMapper _mapper;
        public BannersAdminController(IBannerService bannerService, IMapper mapper)
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
        public async Task<IActionResult> Create([FromBody] BannerRequest bannerRequest, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("BannerRequest can be not valid.");

                if (bannerRequest is null)
                    return BadRequest("BannerRequest can be not null.");

                var banner = _mapper.Map<Banner>(bannerRequest);

                var result = await _bannerService.CreateAsync(banner, cancellationToken);

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
        public async Task<IActionResult> Update([FromBody] BannerRequest bannerRequest, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("BannerRequest can be not valid.");

                if (bannerRequest is null)
                    return BadRequest("BannerRequest can be not null.");

                var banner = _mapper.Map<Banner>(bannerRequest);

                var result = await _bannerService.UpdateAsync(banner, cancellationToken);

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
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var episodes = await _bannerService.GetAllAsync();

                if (!episodes.IsSuccess)
                    return BadRequest(episodes.Error);

                var bannerResponse = _mapper.Map<BannerResponse>(episodes.Value);

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
        [Route("buscar-banner/{id}")]
        [ProducesResponseType(typeof(BannerResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            try
            {
                var episode = await _bannerService.GetByIdAsync(id, cancellationToken);

                if (!episode.IsSuccess)
                    return BadRequest(episode.Error);

                var bannerResponse = _mapper.Map<BannerResponse>(episode.Value);

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
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("BannerRequest can be not valid.");

                if (id is 0)
                    return BadRequest("Banner Id can be not null.");

                var result = await _bannerService.DeleteAsync(id, cancellationToken);

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

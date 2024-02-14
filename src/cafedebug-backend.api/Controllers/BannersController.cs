using AutoMapper;
using cafedebug_backend.application.Response;
using cafedebug_backend.domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace cafedebug_backend.api.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/banner")]
    public class BannersController : ControllerBase
    {
        private readonly IBannerService _bannerService;
        private readonly IMapper _mapper;

        public BannersController(IBannerService bannerService, IMapper mapper)
        {
            _bannerService = bannerService;
            _mapper = mapper;
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
    }
}

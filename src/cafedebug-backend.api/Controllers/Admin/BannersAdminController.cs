using AutoMapper;
using cafedebug.backend.application.Banners.DTOs.Requests;
using cafedebug.backend.application.Banners.DTOs.Responses;
using cafedebug.backend.application.Banners.Services;
using cafedebug_backend.domain.Banners;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cafedebug_backend.api.Controllers.Admin
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/v1/admin-banners")]
    public class BannersAdminController : ControllerBase
    {
        private readonly ILogger<BannersAdminController> _logger;
        private readonly BannerService _bannerService;
        private readonly IMapper _mapper;
        public BannersAdminController(ILogger<BannersAdminController> logger, BannerService bannerService, IMapper mapper)
        {
            _logger = logger;
            _bannerService = bannerService;
            _mapper = mapper;
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(BannerResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] BannerRequest bannerRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("BannerRequest is not valid.");

                if (bannerRequest is null)
                    return BadRequest("BannerRequest cannot be null.");

                var banner = _mapper.Map<Banner>(bannerRequest);
                var result = await _bannerService.CreateAsync(banner);

                if (!result.IsSuccess)
                    return BadRequest(result.Error);

                var bannerResponse = _mapper.Map<BannerResponse>(result.Value);
                return StatusCode(StatusCodes.Status201Created, bannerResponse);
            }
            catch (UnauthorizedAccessException)
            {
                _logger.LogWarning("CreateBanner - Unauthorized access during refresh token.");
                return Unauthorized();
            }
            catch (NullReferenceException ex)
            {
                _logger.LogError(ex, "CreateBanner - Erro create banner");
                return StatusCode(500, "CreateBanner -  Erro create banner");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateBanner - Internal server error.");
                return StatusCode(500, "CreateBanner - Internal server error");
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(BannerResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, [FromBody] BannerRequest bannerRequest)
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

        [HttpGet("{id}")]
        [Authorize]
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

        [HttpDelete("{id}")]
        [Authorize]
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

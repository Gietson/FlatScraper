using System;
using System.Threading.Tasks;
using FlatScraper.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FlatScraper.API.Controllers
{
    [Route("api/[controller]")]
    public class ScrapController : Controller
    {
        private readonly ILogger<ScrapController> _logger;
        private readonly IScraperService _scraperService;

        public ScrapController(IScraperService scraperService, ILogger<ScrapController> logger)
        {
            _scraperService = scraperService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                await _scraperService.ScrapAsync(_logger);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("ScrapController error: {@ex}", ex);
                return BadRequest(ex);
            }
        }
    }
}
using FlatScraper.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using FlatScraper.Infrastructure.DTO;

namespace FlatScraper.API.Controllers
{
    [Route("api/ad")]
    public class AdController : Controller
    {
        private readonly IAdService _adService;

        public AdController(IAdService adService)
        {
            _adService = adService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Get()
        {
            var ads = await _adService.GetAllAsync();

            return Json(ads);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var ads = await _adService.GetAsync(id);
            if (ads == null)
            {
                return NotFound();
            }

            return Json(ads);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] InsertAdDto ad)
        {
            await _adService.AddAsync(ad.Url);

            return Ok();
        }
    }
}

using System;
using System.Threading.Tasks;
using FlatScraper.Infrastructure.DTO;
using FlatScraper.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace FlatScraper.API.Controllers
{
	[Route("api/[controller]")]
	public class AdController : Controller
	{
		private readonly IAdService _adService;

		public AdController(IAdService adService)
		{
			_adService = adService;
		}

		[HttpGet]
		public async Task<IActionResult> Get()
		{
			try
			{
				var ads = await _adService.BrowseAsync();

				return Json(ads);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> Get(Guid id)
		{
			try
			{
				var ads = await _adService.GetAsync(id);
				if (ads == null)
				{
					return NotFound();
				}

				return Json(ads);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPost]
		public async Task<IActionResult> Post([FromBody] AdDto ad)
		{
			try
			{
				await _adService.AddAsync(ad);

				return Ok();
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}
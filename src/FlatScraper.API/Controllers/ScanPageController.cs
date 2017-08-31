using System;
using System.Threading.Tasks;
using FlatScraper.Infrastructure.DTO;
using FlatScraper.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace FlatScraper.API.Controllers
{
    [Route("api/[controller]")]
    public class ScanPageController : Controller
    {
        private readonly IScanPageService _scanPageService;

        public ScanPageController(IScanPageService scanPageService)
        {
            _scanPageService = scanPageService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ScanPageDto page)
        {
            try
            {
                await _scanPageService.AddAsync(page);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var pages = await _scanPageService.GetAllAsync();

                return Json(pages);
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
                var page = await _scanPageService.GetAsync(id);
                return Json(page);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] ScanPageDto page)
        {
            try
            {
                await _scanPageService.UpdateAsync(page);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _scanPageService.RemoveAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
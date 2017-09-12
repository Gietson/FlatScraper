using FlatScraper.Core.Repositories;
using FlatScraper.Infrastructure.Services;
using FluentScheduler;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace FlatScraper.Cron.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        private readonly IScanPageService _scanPageService;
        private readonly IAdRepository _adRepository;
        private static readonly ILogger Logger = Log.Logger;

        public HomeController(IScanPageService scanPageService, IAdRepository adRepository)
        {
            _scanPageService = scanPageService;
            _adRepository = adRepository;
        }

        [HttpGet("")]
        public IActionResult Get()
        {
            try
            {
                JobManager.Initialize(new ScrapRegistry(_scanPageService, _adRepository));
            }
            catch (System.Exception ex)
            {
                Logger.Error("Cron Error: {@ex}", ex);
                return BadRequest(ex);
            }
            return Content("Hi from Cron!");
        }
    }
}

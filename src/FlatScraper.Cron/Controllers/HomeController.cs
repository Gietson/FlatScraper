using FlatScraper.Core.Repositories;
using FlatScraper.Infrastructure.Services;
using FluentScheduler;
using Microsoft.AspNetCore.Mvc;

namespace FlatScraper.Cron.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        private readonly IScanPageService _scanPageService;
        private readonly IAdRepository _adRepository;

        public HomeController(IScanPageService scanPageService, IAdRepository adRepository)
        {
            _scanPageService = scanPageService;
            _adRepository = adRepository;
        }

        [HttpGet("")]
        public IActionResult Get()
        {

            JobManager.Initialize(new ScrapRegistry(_scanPageService, _adRepository));
            return Content("Hi from Cron!");
        }
    }
}

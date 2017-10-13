using System;
using System.Linq;
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
        private static readonly ILogger Logger = Log.Logger;
        private readonly IAdRepository _adRepository;
        private readonly IScanPageService _scanPageService;

        public HomeController(IScanPageService scanPageService, IAdRepository adRepository)
        {
            _scanPageService = scanPageService;
            _adRepository = adRepository;
        }

        [HttpPost("")]
        public IActionResult Post()
        {
            try
            {
                int count = JobManager.AllSchedules.Count();
                DateTime? nextRun = JobManager.GetSchedule("Scrap")?.NextRun;

                if (count == 0)
                {
                    JobManager.Initialize(new ScrapRegistry(_scanPageService, _adRepository));
                    return Ok("Scrap started");
                }
                else
                    return Ok($"Schedules.Count()='{count}', nextRun='{nextRun}'");
            }
            catch (Exception ex)
            {
                Logger.Error("Cron Error: {@ex}", ex);
                return BadRequest(ex);
            }
        }

        [HttpGet("")]
        public IActionResult Get()
        {
            try
            {
                var allSchedules = JobManager.AllSchedules;
                JobExceptionInfo err = null;
                JobManager.JobException += (info) => err = info;
                if (err != null)
                {
                    Logger.Fatal("An error just happened with a scheduled job: {@err}", err);
                    throw new Exception(err.Exception.Message);
                }

                if (allSchedules.Any())
                {
                    return Json(allSchedules);
                }
                else
                {
                    return Content("JobManager doesn't contain elements.");
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Cron Error: {@ex}", ex);
                return BadRequest(ex);
            }
        }

        [HttpDelete("")]
        public IActionResult Delete()
        {
            try
            {
                var schedule = JobManager.GetSchedule("Scrap");
                if (schedule.Disabled)
                {
                    schedule.Enable();
                }
                else
                {
                    schedule.Disable();
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Scrap Delete Ex: {@ex}", ex);
                return BadRequest(ex.Message);
            }
            return Ok();
        }
    }
}
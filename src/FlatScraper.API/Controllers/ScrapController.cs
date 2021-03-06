﻿using System;
using System.Threading.Tasks;
using FlatScraper.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace FlatScraper.API.Controllers
{
    [Route("api/[controller]")]
    public class ScrapController : Controller
    {
        private static readonly ILogger Logger = Log.Logger;
        private readonly IScraperService _scraperService;

        public ScrapController(IScraperService scraperService)
        {
            _scraperService = scraperService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                await _scraperService.ScrapAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                Logger.Error("ScrapController error: {@ex}", ex);
                return BadRequest(ex);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace FlatScraper.API.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        [HttpGet("")]
        public IActionResult Get()
        {

            return Json("get test");
        }

        [HttpPost]
        public IActionResult Post()
        {
            return Json("post ok!");
        }
    }
}
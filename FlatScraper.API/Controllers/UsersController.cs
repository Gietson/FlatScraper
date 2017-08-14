using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FlatScraper.Infrastructure.DTO;
using FlatScraper.Infrastructure.Services;

namespace FlatScraper.API.Controllers
{
    [Route("api/users")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Get()
        {
            var users = await _userService.GetAllAsync();

            return Json(users);
        }

        [HttpGet("{email}")]
        public async Task<IActionResult> Get(string email)
        {
            var user = await _userService.GetAsync(email);
            if (user == null)
            {
                return NotFound();
            }

            return Json(user);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateUserDto user)
        {
            const string role = "user";
            await _userService.RegisterAsync(Guid.NewGuid(), user.Email, user.Username, user.Password, role);

            return Ok();
        }
    }
}
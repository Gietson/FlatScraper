using System;
using System.Threading.Tasks;
using FlatScraper.API.Filters;
using FlatScraper.Common.Authentication;
using FlatScraper.Infrastructure.DTO;
using FlatScraper.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace FlatScraper.API.Controllers
{
    [ValidateModel]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private static readonly ILogger Logger = Log.Logger;
        private readonly IAuthService _authService;
        private readonly IJwtHandler _jwtHandler;
        private readonly IUserService _userService;

        public AuthController(IAuthService authService, IUserService userService, IJwtHandler jwtHandler)
        {
            _authService = authService;
            _userService = userService;
            _jwtHandler = jwtHandler;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] CreateUserDto user)
        {
            try
            {
                await _authService.RegisterAsync(user);
                return Created($"api/auth/register/{user.Email}", null);
            }
            catch (Exception ex)
            {
                Logger.Error("[Register] Error: {@ex}", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginUserDto user)
        {
            try
            {
                await _authService.LoginAsync(user);

                UserDto userAuth = await _userService.GetAsync(user.Email);

                var token = _jwtHandler.CreateToken(userAuth.Id, userAuth.Role);

                return Ok(new {token = token, user = userAuth});
            }
            catch (Exception ex)
            {
                Logger.Error("[Login] Error: {@ex}", ex);
                return BadRequest(ex.Message);
            }
        }
    }
}
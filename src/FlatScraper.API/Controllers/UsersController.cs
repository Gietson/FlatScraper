using System;
using System.Threading.Tasks;
using FlatScraper.Infrastructure.DTO;
using FlatScraper.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace FlatScraper.API.Controllers
{
    [Route("api/[controller]")]
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
            try
            {
                var users = await _userService.GetAllAsync();

                return Ok(users);
            }
            catch (Exception ex)
            {
                //_logger.LogError($"Failed to get All Trips: {ex}");

                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{email}")]
        public async Task<IActionResult> Get(string email)
        {
            try
            {
                var user = await _userService.GetAsync(email);
                if (user == null)
                {
                    return NotFound();
                }

                return Json(user);
            }
            catch (Exception ex)
            {
                //_logger.LogError($"Failed to get All Trips: {ex}");

                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateUserDto user)
        {
            try
            {
                await _userService.RegisterAsync(Guid.NewGuid(), user.Email, user.Username, user.Password, user.Role);

                return Created($"api/users/{user.Email}", null);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _userService.RemoveAsync(id);
            return Ok();
        }
    }
}
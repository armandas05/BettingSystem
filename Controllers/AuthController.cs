using Microsoft.AspNetCore.Mvc;
using BettingSystem.Services;
using BettingSystem.Data.Models;

namespace BettingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly UserService _userService;

        public AuthController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {

            try
            {
                await _userService.RegisterUser(dto);
                return Ok("User succesfully registered!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _userService.LoginUser(dto);
            if (user == null)
            {
                return BadRequest("Invalid credentials");
            }

            HttpContext.Session.SetInt32("UserID", user.UserID);
            HttpContext.Session.SetString("Role", user.Role.ToString());

            return Ok("Logged in succesfully!");
        }



    }
}

using BettingSystem.Services;
using BettingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BettingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("balance")]
        public async Task<ActionResult<decimal>> GetBalance() {

            var userId = HttpContext.Session.GetInt32("UserID");

            if(userId == null)
            {
                return Unauthorized();
            }

            var balance = await _userService.GetBalanceAsync((int)userId);

            return Ok(balance);
        }
    }
}

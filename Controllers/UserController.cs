using BettingSystem.Services;
using Microsoft.AspNetCore.Mvc;


namespace BettingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {

        private readonly UserService _userService;

        public UserController(UserService userService)
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

            var balance = await _userService.GetBalance((int)userId);

            return Ok(balance);

        }





    }
}

using BettingSystem.Data.Models;
using BettingSystem.Services;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace BettingSystem.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : Controller
    {
        private readonly UserService _userService;

        public AdminController(UserService userService)
        {
            _userService = userService;
        }


        [HttpGet("users")]
        public async Task<ActionResult<PagedResult<UserDto>>> GetAllUsers(
            string? searchInput,
            string sortBy = "userid",
            string sortDir = "asc",
            int page = 1,
            int pageSize = 10)
        {
            var userId = HttpContext.Session.GetInt32("UserID");

            if (userId == null) return Unauthorized();

            var users = await _userService.GetUsers(searchInput, sortBy, sortDir, page, pageSize);

            return Ok(users);


        }

        [HttpGet("gamehistories")]
        public async Task<ActionResult<PagedResult<GameHistoryDto>>> GetAllGameHistories(
            string? searchInput,
            string sortBy = "gamesessionid",
            string sortDir = "asc",
            int page = 1,
            int pageSize = 10)
        {

            var userId = HttpContext.Session.GetInt32("UserID");

            if(userId == null) return Unauthorized();

            var gameHistories = await _userService.GetGameHistories(searchInput, sortBy, sortDir, page, pageSize);

            return Ok(gameHistories);

        }






        [HttpDelete("users/{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserID");

            if (userId == null) return Unauthorized();

            await _userService.DeleteUser(id);

            return NoContent();
        }






    }
}

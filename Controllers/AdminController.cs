using BettingSystem.Data.Models;
using BettingSystem.Filters;
using BettingSystem.Services;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace BettingSystem.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [AdminOnly]
    public class AdminController : Controller
    {
        private readonly UserService _userService;
        private readonly TransactionService _transactionService;
        private readonly AnalyticsService _analyticsService;

        public AdminController(UserService userService, TransactionService transactionService, AnalyticsService analyticsService)
        {
            _userService = userService;
            _transactionService = transactionService;
            _analyticsService = analyticsService;
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

        [HttpGet("transactions")]
        public async Task<ActionResult<PagedResult<TransactionDto>>> GetAllTransactions(
            string? searchInput,
            string sortBy = "transactionid",
            string sortDir = "asc",
            int page = 1,
            int pageSize = 10)
        {
            var userId = HttpContext.Session.GetInt32("UserID");

            if (userId == null) return Unauthorized();

            var transactions = await _transactionService.GetAllTransactions(searchInput, sortBy, sortDir, page, pageSize);

            return Ok(transactions);


        }

        [HttpDelete("users/{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserID");

            if (userId == null) return Unauthorized();

            await _userService.DeleteUser(id);

            return NoContent();
        }


        [HttpGet("stats")]
        public async Task<ActionResult> GetAnalytics(int days = 7)
        {
            var userId = HttpContext.Session.GetInt32("UserID");

            if(userId == null) return Unauthorized();

            var analytics = await _analyticsService.GetAnalytics(days);

            return Ok(analytics);



        }

    }
}

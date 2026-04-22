using BettingSystem.Services;
using Microsoft.AspNetCore.Mvc;
using BettingSystem.Data.Models;
using BettingSystem.Services.Interfaces;

namespace BettingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : Controller
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService) {
            _transactionService = transactionService;
        }

        [HttpPost("deposit")]
        public async Task<ActionResult> Deposit([FromBody] DepositDto dto)
        {
            var userId = HttpContext.Session.GetInt32("UserID");

            if (userId == null) return Unauthorized();

            await _transactionService.DepositAsync(userId.Value, dto.DepositAmount, dto.Method);

            return Ok();
        }
    }
}

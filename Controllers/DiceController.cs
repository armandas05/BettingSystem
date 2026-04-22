using BettingSystem.Data.Models;
using BettingSystem.Services;
using BettingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BettingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DiceController : ControllerBase
    {
        private readonly IGameService _gameService;

        public DiceController(IGameService gameService) {
            _gameService = gameService;
        }

        [HttpPost("rolldice")]
        public async Task<ActionResult> RollDice([FromBody] GameDto dto)
        {
            var userId = HttpContext.Session.GetInt32("UserID");

            if (userId == null)
            {
                return Unauthorized();
            }

            if (dto.BetAmount <= 0)
            {
                return BadRequest("Invalid bet amount!");
            }

            var result = await _gameService.PlayDiceAsync(dto, (int)userId);

            return Ok(result);
        }
    }
}

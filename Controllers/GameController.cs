using BettingSystem.Data.Models;
using BettingSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace BettingSystem.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class GameController : Controller
    {
        
        private readonly GameService _gameService;

        public GameController(GameService gameService)
        {
            _gameService = gameService;
        }

        [HttpPost]
        public async Task<IActionResult> PlayGame([FromBody] GameDto dto)
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

            var result = await _gameService.PlayDice(dto, (int)userId);

            return Ok(result);



        }



    }
}

using BettingSystem.Data.Models;
using BettingSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace BettingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DiceController : ControllerBase
    {

        private readonly GameService _gameService;

        public DiceController(GameService gameService) {
            _gameService = gameService;
        }


        [HttpPost("rolldice")]
        public async Task<ActionResult> RollDice([FromBody] GameDto dto)
        {

            var userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null) return Unauthorized();


            var result = await _gameService.PlayDice(dto, (int)userId);
            


            return Ok(result);
        }









    }
}

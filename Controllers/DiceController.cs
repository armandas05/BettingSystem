using BettingSystem.Data.Models;
using BettingSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace BettingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DicesController : ControllerBase
    {

        private readonly UserService _userService;
        private readonly DiceService _diceService;

        public DicesController(UserService userService, DiceService diceService) {
            _userService = userService;
            _diceService = diceService;
        }


        [HttpPost("rolldice")]
        public async Task<ActionResult> RollDice([FromBody] GameInfoDto dto)
        {

            var userId = HttpContext.Session.GetInt32("UserID");

            if (userId == null) return Unauthorized();

            if(dto.betAmount <= 0 || dto.betAmount >= 99999)
            {
                return BadRequest("Invalid bet amount!");
            }

            if (string.IsNullOrEmpty(dto.rollType))
            {
                return BadRequest("Invalid roll type");
            }

            var balance = await _userService.GetBalance((int)userId);

            if (balance < dto.betAmount)
            {
                return BadRequest("Not enough balance!");
            }

            var result = await _diceService.RollDice((int)userId, dto.betAmount, dto.rollNumber, dto.rollType);

            return Ok(result);
        }









    }
}

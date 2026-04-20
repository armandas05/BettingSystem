using BettingSystem.Data.Entities;
using BettingSystem.Data.Models;
using BettingSystem.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BettingSystem.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class BlackjackController : ControllerBase
    {
        private readonly DeckService _deckService;
        private readonly BlackjackService _blackjackService;
        private readonly UserService _userService;
        private readonly GameService _gameService;


        public BlackjackController(
            DeckService deckService, 
            BlackjackService blackjackService, 
            UserService userService,
            GameService gameService)
        {
            _deckService = deckService;
            _blackjackService = blackjackService;
            _userService = userService;
            _gameService = gameService;
        }


        [HttpGet("drawcard")]
        public ActionResult<Card> GetCard() => _deckService.DrawCard();

        [HttpGet("dealerhand")]
        public ActionResult<IEnumerable<Card>> ShowDealerHand() => _blackjackService.ShowDealerCards();

        [HttpGet("playerhand")]
        public ActionResult<IEnumerable<Card>> ShowPlayerHand() => _blackjackService.ShowPlayerCards();

        [HttpPost("hit")]
        public ActionResult Hit() {

            _blackjackService.Hit();

            return Ok("Player has succesfully hitted!");
        }

        [HttpPost("stand")]
        public ActionResult Stand()
        {
            _blackjackService.Stand();

            return Ok("Player has succesfully standed!");
        }

        [HttpPost("bet")]
        public async Task<ActionResult> PlaceBet([FromBody] decimal betAmount)
        {
            if(betAmount < 0) return BadRequest("Bet must be positive!");

            var userId = HttpContext.Session.GetInt32("UserID");

            if(userId == null)
            {
                return Unauthorized();
            }

            var success = await _userService.PlaceBet(userId.Value, betAmount);

            if(!success)
            {
                return BadRequest("Not enough balance!");
            }

            _blackjackService.SetBet(betAmount);


            return Ok();

        }


        [HttpGet("status")]
        public ActionResult<string> GameStatus()
        {
            return _blackjackService.CheckGameStatus();
        }

        [HttpPost("restart")]
        public ActionResult RestartGame()
        {
            _blackjackService.RestartGame();
            return Ok();
        }



        [HttpGet("playerscore")]
        public ActionResult<int> GetPlayerScore() {
            return Ok(_blackjackService.GetPlayerScore());
        } 

        [HttpGet("dealerscore")]
        public ActionResult<int> GetDealerScore()
        {
            return Ok(_blackjackService.GetDealerScore());
        }
        



        [HttpPost("startgame")]
        public ActionResult StartGame()
        {

            _blackjackService.StartGame();
            return Ok();


        }

        [HttpPost("finishgame")]
        public async Task<ActionResult> FinishGame()
        {
            var userId = HttpContext.Session.GetInt32("UserID");

            if(userId == null)
            {
                return Unauthorized();
            }

            var result = _blackjackService.CheckGameStatus();
            var bet = _blackjackService.GetBet();

            var dto = new GameResult();

            _blackjackService.RestartGame();

            if(result == "Player wins!")
            {
                dto.Result = WinType.Win;
                dto.WonAmount = bet * 2;
                var results = await _gameService.FinishBlackjack(dto, (int)userId);
            } 
            else if (result == "Dealer wins!")
            {
                dto.Result = WinType.Lose;
                dto.WonAmount = 0;
                var results = await _gameService.FinishBlackjack(dto, (int)userId);
            }
            else
            {
                dto.Result = WinType.Draw;
                dto.WonAmount = 0;
                var results = await _gameService.FinishBlackjack(dto, (int)userId);
            }


            return Ok(result);

        }
    


    }
}
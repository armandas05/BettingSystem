using BettingSystem.Data;
using BettingSystem.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Threading.Channels;

namespace BettingSystem.Services
{
    public class DiceService
    {

        private readonly DiceGameService _diceGameService;

        public DiceService(AppDbContext context, UserService userService)
        {
            _diceGameService = new DiceGameService();
        }


        public GameResult Play(decimal betAmount, decimal rollNumber, string rollType)
        {

            var rolled = _diceGameService.GetDiceNumber();

            bool win = rollType == "Under"
                ? rolled < rollNumber
                : rolled > rollNumber;

            var multi = GetMultiplier(rollNumber, rollType);

            var amountWon = win ? betAmount * multi : 0;

            return new GameResult
            {
                Rolled = rolled,
                WonAmount = amountWon,
                Result = win ? WinType.Win : WinType.Lose
            };

            

        }

        public decimal GetMultiplier(decimal rollNumber, string rollType)
        {
            var chance = rollType == "Under" ? rollNumber : 100 - rollNumber;

            chance = Math.Clamp(chance, 0.01m, 95.00m);

            var multi = 95.00m / chance;

            if(multi == 1.00m && chance == 95.00m)
            {
                multi = 1.01m;
            }

            return Math.Round(multi, 2);
        }

      

    }
}

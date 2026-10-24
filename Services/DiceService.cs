using BettingSystem.Domain;
using BettingSystem.Data;
using BettingSystem.Data.Models;
using BettingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Threading.Channels;

namespace BettingSystem.Services
{
    public class DiceService : IDiceService
    {

        private readonly DiceGameService _diceGameService;

        public DiceService(DiceGameService diceGameService)
        {
            _diceGameService = diceGameService;
        }
        public GameResult Play(decimal betAmount, decimal rollNumber, string rollType)
        {
            BettingSystem.Domain.Validation.PositiveAmount(betAmount, name: "Bet");
            if (rollNumber is <= DiceConstants.MinRoll or >= DiceConstants.PercentageScale || rollType is not ("Under" or "Over"))
                throw new BettingSystem.Domain.BettingValidationException("Invalid dice selection.");

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
            var chance = rollType == "Under" ? rollNumber : DiceConstants.PercentageScale - rollNumber;

            chance = Math.Clamp(chance, DiceConstants.MinWinChance, DiceConstants.MaxWinChance);

            var multi = DiceConstants.PayoutPercentage / chance;

            if (multi == DiceConstants.BreakEvenMultiplier && chance == DiceConstants.MaxWinChance)
            {
                multi = DiceConstants.MinPayoutMultiplier;
            }

            return Math.Round(multi, DiceConstants.DecimalPlaces);
        }
    }
}

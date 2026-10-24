using BettingSystem.Domain;
using System.Security.Cryptography;

namespace BettingSystem.Services
{
    public class DiceGameService
    {
        public decimal GetDiceNumber()
        {
            var bytes = new byte[sizeof(uint)];
            RandomNumberGenerator.Fill(bytes);

            uint randomInt = BitConverter.ToUInt32(bytes, 0);

            decimal normalized = randomInt / (decimal)uint.MaxValue;

            decimal result = normalized * DiceConstants.MaxRoll;

            return Math.Round(result, DiceConstants.DecimalPlaces);
        }
    }
}

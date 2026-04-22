using System.Security.Cryptography;

namespace BettingSystem.Services
{
    public class DiceGameService
    {
        public decimal GetDiceNumber()
        {
            var bytes = new byte[4];
            RandomNumberGenerator.Fill(bytes);

            uint randomInt = BitConverter.ToUInt32(bytes, 0);

            decimal normalized = randomInt / (decimal)uint.MaxValue;

            decimal result = normalized * 99.99m;

            return Math.Round(result, 2);
        }
    }
}

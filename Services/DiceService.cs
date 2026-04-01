using BettingSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Channels;

namespace BettingSystem.Services
{
    public class DiceService
    {

        private readonly AppDbContext _context;
        private readonly UserService _userService;
        private readonly DiceGameService _diceGameService;

        public DiceService(AppDbContext context, UserService userService)
        {
            _context = context;
            _userService = userService;
            _diceGameService = new DiceGameService();
        }


        public async Task<IResult> RollDice(int userId, decimal betAmount, decimal rollNumber, string rollType)
        {

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserID == userId);

            if (user == null)
            {
                return Results.NotFound("No account registered with this email!");
            }

            if (rollType != "Under" && rollType != "Over")
            {
                return Results.BadRequest("Roll type is invalid!");
            }

            if (rollType == "Under" && (rollNumber > 95 || rollNumber < 0.01m))
            {
                return Results.BadRequest("Roll number is invalid!");
            }

            if (rollType == "Over" && (rollNumber < 5.99m || rollNumber > 99.99m))
            {
                return Results.BadRequest("Roll number is invalid!");
            }

            var balance = await _userService.GetBalance(user.UserID);

            if (balance <= 0 || balance < betAmount)
            {
                return Results.BadRequest("Not enough balance!");
            }

            var rolled = _diceGameService.GetDiceNumber();

            bool win;

            user.Balance -= betAmount;

            if(rollType == "Under")
            {
                win = rolled < rollNumber;
            }
            else
            {
                win = rolled > rollNumber;
            }
            
            var multi = GetMultiplier(rollNumber, rollType);

            if (win)
            {
                await _userService.AddBalance(user.UserID, betAmount * multi);
            }

            await _context.SaveChangesAsync();


            return Results.Ok(rolled);

        }

        public decimal GetMultiplier(decimal rollNumber, string rollType)
        {
            var chance = 0.00m;

            if (rollType == "Under")
            {
                chance = rollNumber;
            }
            else
            {
                chance = 100 - rollNumber;
            }

            if (chance > 94.00m) chance = 95.00m;
            if (chance < 0.01m) chance = 0.01m;

            var multi = 95.00m / chance;

            if ((multi == 1.00m) && (chance == 95.00m)) multi = 1.01m;

            multi = Math.Round(multi, 2);

            return multi;
        }




    }
}

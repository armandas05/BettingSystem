using BettingSystem.Data;
using BettingSystem.Data.Models;

public static class DbSeeder
{
    public static void Seed(AppDbContext context)
    {
        if (!context.Users.Any())
        {
            context.Users.AddRange(
                new User
                {
                    FirstName = "Admin",
                    LastName = "User",
                    Email = "admin@test.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("123456"),
                    Balance = 1000,
                    Role = 1,
                    IsVerified = true
                },
                new User
                {
                    FirstName = "Test",
                    LastName = "User",
                    Email = "user@test.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("123456"),
                    Balance = 500,
                    Role = 0,
                    IsVerified = true
                }
            );

            context.SaveChanges();
        }

        if (!context.GameHistories.Any())
        {
            context.GameHistories.AddRange(
                new GameHistory { UserID = 2, BetAmount = 50, AmountWon = 100, Result = 0, GameID = 1 },
                new GameHistory { UserID = 2, BetAmount = 20, AmountWon = 0, Result = 1, GameID = 2 }
            );
        }

        if (!context.Transactions.Any())
        {
            context.Transactions.Add(
                new Transaction { UserID = 2, DepositAmount = 200, Method = 3 }
            );
        }

        context.SaveChanges();
    }
}
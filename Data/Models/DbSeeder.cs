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
                    Role = User.UserRoles.Admin,
                    IsVerified = true
                },
                new User
                {
                    FirstName = "Test",
                    LastName = "User",
                    Email = "user@test.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("123456"),
                    Balance = 500,
                    Role = User.UserRoles.User,
                    IsVerified = true
                }
            );

            context.SaveChanges();
        }

        context.SaveChanges();
    }
}
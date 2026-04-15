using Microsoft.EntityFrameworkCore;
using BettingSystem.Data.Models;

namespace BettingSystem.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<GameHistory> GameHistories { get; set; }
        public DbSet<GameInformation> GameInformations { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GameInformation>().HasData(
                new GameInformation
                {
                    GameID = 1,
                    GameName = "Blackjack",
                    GameDescription = "A game of blackjack..."
                },
                new GameInformation
                {
                    GameID = 2,
                    GameName = "Dices",
                    GameDescription = "Roll the dice..."
                }
            );
        }
    }
}

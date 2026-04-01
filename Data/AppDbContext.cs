using Microsoft.EntityFrameworkCore;
using BettingSystem.Data.Models;

namespace BettingSystem.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<GameHistory> GameHistories { get; set; }
        public DbSet<GameInformation> GameInformations { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    }
}

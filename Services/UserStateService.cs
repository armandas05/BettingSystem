using BettingSystem.Data.Models;
using BettingSystem.Services.Interfaces;

namespace BettingSystem.Services
{
    public class UserStateService : IUserStateService
    {
        public User? CurrentUser { get; private set; }
        public void Login(User user) => CurrentUser = user;
        public void Logout() => CurrentUser = null;
        public bool IsLoggedIn => CurrentUser != null;
    }
}

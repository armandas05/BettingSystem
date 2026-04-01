using BettingSystem.Data.Models;

namespace BettingSystem.Services
{
    public class UserStateService
    {
        public User? CurrentUser { get; private set; }

        public void Login(User user) => CurrentUser = user;
        public void Logout() => CurrentUser = null;

        public bool IsLoggedIn => CurrentUser != null;
    }
}

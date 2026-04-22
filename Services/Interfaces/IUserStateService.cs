using BettingSystem.Data.Models;

namespace BettingSystem.Services.Interfaces
{
    public interface IUserStateService
    {
        void Login(User user);
        void Logout();
    }
}

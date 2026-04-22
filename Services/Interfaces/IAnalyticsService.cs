using BettingSystem.Data.Models;

namespace BettingSystem.Services.Interfaces
{
    public interface IAnalyticsService
    {
        Task<AnalyticsDto> GetAnalyticsAsync(int days);
    }

}

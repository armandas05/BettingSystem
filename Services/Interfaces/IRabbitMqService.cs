using BettingSystem.Data.Models;

namespace BettingSystem.Services.Interfaces
{
    public interface IRabbitMqService
    {
        Task PublishAsync<T> (string queueName, T message);
    }
}

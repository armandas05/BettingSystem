using BettingSystem.Data;
using BettingSystem.Data.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace BettingSystem.Services.RabbitMQ
{
    public class GameConsumer : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public GameConsumer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory { HostName = "localhost" };

            var connection = await factory.CreateConnectionAsync();
            var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queue: "game-history",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);

                var dto = JsonSerializer.Deserialize<GameEventDto>(json);


                using (var scope = _serviceProvider.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                    db.GameHistories.Add(new GameHistory
                    {
                        UserID = dto.UserID,
                        DateTime = dto.DateTime,
                        BetAmount = dto.BetAmount,
                        AmountWon = dto.AmountWon,
                        Result = dto.Result,
                        GameID = dto.GameId

                    });

                    await db.SaveChangesAsync();
                }
            };

            await channel.BasicConsumeAsync(
                queue: "game-history",
                autoAck: true,
                consumer: consumer
            );

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
    }
}

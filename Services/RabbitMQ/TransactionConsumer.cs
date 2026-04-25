using BettingSystem.Data;
using BettingSystem.Data.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace BettingSystem.Services.RabbitMQ
{
    public class TransactionConsumer : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public TransactionConsumer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory { HostName = "localhost" };

            var connection = await factory.CreateConnectionAsync();
            var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queue: "transaction-history",
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

                var dto = JsonSerializer.Deserialize<TransactionEventDto>(json);

                using (var scope = _serviceProvider.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                    db.Transactions.Add(new Transaction
                    {
                        UserID = dto.UserID,
                        DepositAmount = dto.DepositAmount,
                        Method = dto.Method,
                        DateDeposited = dto.DateDeposited,
                    });

                    await db.SaveChangesAsync();
                }
            };

            await channel.BasicConsumeAsync(
                queue: "transaction-history",
                autoAck: true,
                consumer: consumer
            );

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
    }
}

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using ApiGateway.Data;
using ApiGateway.DTO;

namespace ApiGateway.Services.Rabbit
{
    public class RabbitMQConsumer : BackgroundService
    {
        private IConnection _connection;
        private IModel _channel;
        private readonly IServiceProvider _serviceProvider;

        public RabbitMQConsumer(IServiceProvider serviceProvider)
            {
                _serviceProvider = serviceProvider;

                var factory = new ConnectionFactory() { HostName = "localhost" };
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.QueueDeclare(
                    queue: "estoque_update_queue",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);
            }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                try
                {
                    var update = JsonSerializer.Deserialize<EstoqueUpdateDto>(message);

                    using var scope = _serviceProvider.CreateScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                    var product = await dbContext.Produto
                        .FirstOrDefaultAsync(p => p.Productid == update.ProductId);

                    if (product != null)
                    {
                        product.Stock -= update.Quantity;
                        await dbContext.SaveChangesAsync();

                        Console.WriteLine($"[✔] Estoque atualizado para produto {product.Productid}.");
                    }
                    else
                    {
                        Console.WriteLine($"[!] Produto {update.ProductId} não encontrado.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Erro] Falha ao processar mensagem: {ex.Message}");
                }
            };
            _channel.BasicConsume(
                    queue: "estoque_update_queue",
                    autoAck: true,
                    consumer: consumer);

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
    }

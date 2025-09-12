using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using ApiGateway.Data;
using ApiGateway.DTO;
using ApiGateway.Models;

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
                    var update = JsonSerializer.Deserialize<EstoqueUpdateDTO>(message);


                    using var scope = _serviceProvider.CreateScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                    var pedido = await dbContext.Pedido
                     .Include(p => p.Product)
                     .Include(p => p.Client)
                     .FirstOrDefaultAsync(ped => ped.IdPedido == update.PedidoId);


                    if (pedido != null && pedido.status == Enum.PedidosEnum.requisitado)
                    {
                        var pedidoHistoric = new PedidoHistoricStatus();

                        pedido.status = Enum.PedidosEnum.processando;
                        pedidoHistoric.Status = Enum.PedidosEnum.processando;
                        pedidoHistoric.PedidosIdPedidos = pedido.IdPedido;
                        dbContext.Update(pedido);
                        dbContext.Add(pedidoHistoric);
                        await dbContext.SaveChangesAsync();
                    }


                    if (pedido != null && pedido.Product != null && pedido.Client != null)
                    {
                        if (pedido.status == Enum.PedidosEnum.processando)
                        {
                            var notification = new Notification();
                            var pedidoHistoric = new PedidoHistoricStatus();

                            if (pedido.Product.Stock - pedido.quantidade < 0)
                            {
                                notification.Product = pedido.Product;
                                notification.Client = pedido.Client;
                                notification.NotificationMenssage = "Não há essa quantidade de produtos";

                                pedido.status = Enum.PedidosEnum.rejeitado;
                                pedidoHistoric.Status = Enum.PedidosEnum.rejeitado;
                                pedidoHistoric.PedidosIdPedidos = pedido.IdPedido;
                                dbContext.Update(pedido);
                                dbContext.Add(pedidoHistoric);
                                await dbContext.SaveChangesAsync();
                            }
                            else
                            {
                                pedido.Product.Stock -= pedido.quantidade;
                                notification.Product = pedido.Product;
                                notification.Client = pedido.Client;
                                notification.NotificationMenssage = "Produto encontrado e reservado";
                                pedido.status = Enum.PedidosEnum.confirmado;
                                pedidoHistoric.Status = Enum.PedidosEnum.confirmado;
                                pedidoHistoric.PedidosIdPedidos = pedido.IdPedido;
                                dbContext.Update(pedido);
                                dbContext.Add(pedidoHistoric);
                                await dbContext.SaveChangesAsync();


                            }

                            dbContext.Add(notification);
                            await dbContext.SaveChangesAsync();
                            Console.WriteLine($"[✔] Estoque atualizado para produto {pedido.Product.Productid}.");
                        }
                        else
                        {
                            Console.WriteLine($"[⏳] Pedido {pedido.IdPedido} ainda está dentro do tempo de espera. Ignorando por enquanto.");
                        }

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

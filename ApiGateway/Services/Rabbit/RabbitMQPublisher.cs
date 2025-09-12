using System.Text;
using System.Text.Json;
using ApiGateway.DTO;
using RabbitMQ.Client;

namespace ApiGateway.Services.Rabbit
{
    public class RabbitMQPublisher : IRabbitMQPublisher
    {
        private readonly IConfiguration _config;

        public RabbitMQPublisher(IConfiguration config)
        {
            _config = config;
        }

        public void PublicarPedido(EstoqueUpdateDTO message)
        {
            var factory = new ConnectionFactory
            {
                HostName = _config["RabbitMQ:HostName"] ?? "localhost"
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            // Nome da fila deve ser idêntico ao usado pelo consumer
            string queueName = "estoque_update_queue";

            channel.QueueDeclare(queue: queueName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

            channel.BasicPublish(exchange: "",
                                 routingKey: queueName,
                                 basicProperties: null,
                                 body: body);

            Console.WriteLine($"[✔] Mensagem publicada na fila: {queueName}");
        }
    }
}

using ApiGateway.DTO;

namespace ApiGateway.Services.Rabbit
{
    public interface IRabbitMQPublisher
    {
        void PublicarPedido(EstoqueUpdateDTO message);
    }
}

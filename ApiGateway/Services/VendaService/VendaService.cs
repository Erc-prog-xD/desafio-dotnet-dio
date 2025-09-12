using ApiGateway.Data;
using ApiGateway.DTO;
using ApiGateway.Models;
using ApiGateway.Services.Rabbit;
using Microsoft.EntityFrameworkCore;

namespace ApiGateway.Services.VendaService
{
    public class VendaService : IVendaService
    {
        private readonly AppDbContext _context;
        private readonly IRabbitMQPublisher _publisher;


        public VendaService(AppDbContext context, IRabbitMQPublisher publisher)
        {
            _context = context;
            _publisher = publisher;
        }

        public async Task<Response<Pedidos>> registrarPedido(string email, int codigoProduto, int quantidade)
        {
            var response = new Response<Pedidos>();
            var pedidoHistoric = new PedidoHistoricStatus();
            var pedido = new Pedidos();
            var produto = await _context.Produto
                .FirstOrDefaultAsync(p => p.Productid == codigoProduto);
            var client = await _context.Client
                .FirstOrDefaultAsync(p => p.Email == email);

            if (produto.Stock < quantidade)
            {
                response.Dados = null;
                response.Status = false;
                response.Mensage = "A quantidade ultrapassa o stock disponível";
                return response;
            }

            pedido.Client = client;
            pedido.Product = produto;
            pedido.status = Enum.PedidosEnum.requisitado;
            pedido.quantidade = quantidade;
            _context.Add(pedido);
            await _context.SaveChangesAsync();

            pedidoHistoric.Status = Enum.PedidosEnum.requisitado;
            pedidoHistoric.PedidosIdPedidos = pedido.IdPedido;
            _context.Add(pedidoHistoric);
            await _context.SaveChangesAsync();


            // Criar a mensagem para o RabbitMQ
            var updateMessage = new EstoqueUpdateDTO
            {
                PedidoId = pedido.IdPedido
            };
            // Enviar para a fila
            _publisher.PublicarPedido(updateMessage);

            response.Dados = pedido;
            response.Status = true;
            response.Mensage = "Pedido salvo";
            return response;
        }

        public async Task<Response<List<PedidoDTO>>> consultarPedidos(string email)
        {
            var response = new Response<List<PedidoDTO>>();

            var client = await _context.Client
                .FirstOrDefaultAsync(c => c.Email == email);

            if (client == null)
            {
                response.Status = false;
                response.Mensage = "Cliente não encontrado";
                response.Dados = null;
                return response;
            }

            var pedidos = await _context.Pedido
                .Where(p => p.Client.Id == client.Id)
                .Include(p => p.Product)
                .Include(p => p.Client)
                .ToListAsync();

            var pedidosDto = pedidos.Select(p => new PedidoDTO
            {
                IdPedido = p.IdPedido,
                NomeProduto = p.Product?.Name,
                EmailCliente = p.Client?.Email,
                Quantidade = p.quantidade,
                Status = p.status.ToString(),
                DataCriacao = p.CreationDate
            }).ToList();

            response.Status = true;
            response.Mensage = "Pedidos encontrados";
            response.Dados = pedidosDto;

            return response;
        }
    }
}

using ApiGateway.DTO;
using ApiGateway.Models;

namespace ApiGateway.Services.VendaService
{
    public interface IVendaService
    {
        Task<Response<Pedidos>> registrarPedido(string email, int codigoProduto, int quantidade);
        Task<Response<List<PedidoDTO>>> consultarPedidos(string email);
    }
}

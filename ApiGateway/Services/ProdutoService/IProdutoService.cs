using ApiGateway.Dto;
using ApiGateway.Models;

namespace ApiGateway.Services.ProdutoService
{
    public interface IProdutoService
    {
        Task<Response<Product>> RegistrarProduto(Product produto);
        Task<Response<List<Product>>> getProducts(ProdutoDTO filtro);
    }
}

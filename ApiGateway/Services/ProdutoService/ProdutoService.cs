using ApiGateway.Data;
using ApiGateway.Dto;
using ApiGateway.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiGateway.Services.ProdutoService
{
    public class ProdutoService
    {
        
        private readonly AppDbContext _context;
        public ProdutoService(AppDbContext context)
        {
            _context = context;

        }
        public async Task<Response<Product>> RegistrarProduto(Product produto)
        {
            Response<Product> response = new Response<Product>();
            _context.Add(produto);
            await _context.SaveChangesAsync();
            response.Status = true;
            response.Mensage = "produto criado";
            response.Dados = null;
            return response;
        }

        public async Task<Response<List<Product>>> getProducts(ProdutoDTO filtro)
        {   
            var response = new Response<List<Product>>();

            if (string.IsNullOrEmpty(filtro.name) && filtro.price == null && filtro.id == null)
            {
                response.Status = false;    
                response.Mensage = "Nenhum filtro fornecido. Retornando todos os produtos.";
                response.Dados = null;
                return response;
            }
            
            // Começa a query base
            var query = _context.Produto.AsQueryable();


            // Adiciona filtros dinamicamente
            if (!string.IsNullOrEmpty(filtro.name))
            {
                query = query.Where(p => p.Name.Contains(filtro.name));
            }
            if (filtro.id != null)
            {
                query = query.Where(p => p.Productid == filtro.id);
            }
            // Filtro por preço (igualdade exata)
            if (filtro.price > 0)
            {
                query = query.Where(p => p.Price == filtro.price);
            }


            // Executa a query
            var produtos = await query.ToListAsync();

            // Prepara a resposta
            response.Dados = produtos;
            response.Mensage = produtos.Any() ? "Produtos encontrados." : "Nenhum produto encontrado.";
            response.Status = true;

            return response;
        }

    }
}
    

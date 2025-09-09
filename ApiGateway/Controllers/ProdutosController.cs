using ApiGateway.Dto;
using ApiGateway.Models;
using ApiGateway.Services.AuthService;
using ApiGateway.Services.ProdutoService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {

        private readonly ProdutoService _produtoService;

        public ProdutosController(ProdutoService produtoService)
        {
            _produtoService = produtoService;
        }

        [HttpPost("RegistrarProduto")]
        public async Task<ActionResult<Response<string>>> registerProductAsync(Product produto)
        {
            var response = await _produtoService.RegistrarProduto(produto);
            return Ok(response);
        }

        [HttpGet("ConsultarProdutos")]
        public async Task<ActionResult<Response<List<Product>>>> GetProducts([FromQuery] int? id, [FromQuery] string? name, [FromQuery] decimal? price)
        {
            var filtro = new ProdutoDTO { id = id, name = name, price = price};
            var response = await _produtoService.getProducts(filtro);
            return Ok(response);
        }

    }
}

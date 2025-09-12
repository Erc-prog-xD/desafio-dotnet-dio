using ApiGateway.Models;
using ApiGateway.Services.ProdutoService;
using ApiGateway.Services.VendaService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendasController : ControllerBase
    {
        private readonly IVendaService _vendaService;

        public VendasController(IVendaService vendaService)
        {
            _vendaService = vendaService;
        }

        [Authorize]
        [HttpPost("RealizarPedido")]
        public async Task<ActionResult<Response<string>>> registerPedidoAsync(int codigoProduto, int quantidade)
        {
            var email = User.FindFirst("email")?.Value;

            var result = await _vendaService.registrarPedido(email, codigoProduto, quantidade);
            return Ok(result);
        }
        [Authorize]
        [HttpGet("ConsultarPedidos")]
        public async Task<ActionResult<Response<string>>> ConsultPedidoAsync()
        {
            var email = User.FindFirst("email")?.Value;

            var result = await _vendaService.consultarPedidos(email);
            return Ok(result);
        }
    }
}

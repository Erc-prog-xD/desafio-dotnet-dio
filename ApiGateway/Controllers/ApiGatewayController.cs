using ApiGateway.Dto;
using ApiGateway.Models;
using ApiGateway.Services.AuthService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ApiGateway.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ApiGatewayController : ControllerBase
    {


        [Authorize]
        [HttpGet("ConsultPedidos")]
        public ActionResult<Response<string>> consultDados(){
            Response<string> response = new Response<string>();
            response.Mensage = "Acessei";
            return response;
        }
    }
}

using ApiGateway.Dto;
using ApiGateway.Services.AuthService;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthInterface _authInterface;
        public AuthController(IAuthInterface authInterface)
        {
            _authInterface = authInterface;
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login(ClientLoginDTO clientLogin)
        {
            var response = await _authInterface.Login(clientLogin);
            return Ok(response);
        }

        [HttpPost("Register")]
        public async Task<ActionResult> Register(ClientCriacaoDTO clientRegister)
        {
            var response = await _authInterface.Registrar(clientRegister);
            return Ok(response);
        }
    }
}

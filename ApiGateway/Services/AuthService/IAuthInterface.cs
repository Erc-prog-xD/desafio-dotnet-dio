using ApiGateway.Dto;
using ApiGateway.Models;

namespace ApiGateway.Services.AuthService
{
    public interface IAuthInterface
    {
        Task<Response<ClientCriacaoDTO>> Registrar(ClientCriacaoDTO clientRegister);
        Task<Response<string>> Login(ClientLoginDTO clientLogin);

    }
}

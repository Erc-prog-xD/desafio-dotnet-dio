using System.ComponentModel.DataAnnotations;

namespace ApiGateway.Dto
{
    public class ClientLoginDTO
    {
        [Required(ErrorMessage = "O campo Email é obrigatório."), EmailAddress(ErrorMessage = "Email invalido!")]
        public string email { get; set; }
        [Required(ErrorMessage = "O campo Password é obrigatório.")]
        public string password { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace ApiGateway.Dto
{
    public class ClientCriacaoDTO
    {
        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "O campo Email é obrigatório."), EmailAddress(ErrorMessage ="Email invalido!")]
        public string Email { get; set; }
        [Required(ErrorMessage = "O campo Password é obrigatório.")]
        public string Password { get; set; }

    }
}

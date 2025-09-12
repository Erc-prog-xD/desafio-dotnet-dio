using ApiGateway.Enum;
using System.ComponentModel.DataAnnotations;

namespace ApiGateway.Models
{
    public class Pedidos
    {
        [Key]
        public int IdPedido { get; set; }
        public Product Product { get; set; }
        public Client Client { get; set; }
        public PedidosEnum status { get; set; }
        public int quantidade { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
    }
}

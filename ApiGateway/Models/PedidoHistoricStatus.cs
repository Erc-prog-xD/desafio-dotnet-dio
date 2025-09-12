using ApiGateway.Enum;
using System.ComponentModel.DataAnnotations;

namespace ApiGateway.Models
{
    public class PedidoHistoricStatus
    {
        [Key]
        public int PedidoHistoricStatusId { get; set; }
        public int PedidosIdPedidos { get; set; }
        public PedidosEnum Status { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;

    }
}

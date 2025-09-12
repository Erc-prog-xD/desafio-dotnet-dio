namespace ApiGateway.DTO
{
    public class PedidoDTO
    {
        public int IdPedido { get; set; }
        public string NomeProduto { get; set; }
        public string EmailCliente { get; set; }
        public int Quantidade { get; set; }
        public string Status { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}

namespace ApiGateway.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public bool authorized { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}

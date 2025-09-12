namespace ApiGateway.Models
{
    public class Notification
    {
        public int NotificationId { get; set; }
        public Client Client { get; set; }
        public Product Product { get; set; }
        public string NotificationMenssage{ get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
    }
}

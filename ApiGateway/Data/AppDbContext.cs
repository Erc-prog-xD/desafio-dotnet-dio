using ApiGateway.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiGateway.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Client> Client { get; set; }
        public DbSet<Product> Produto { get; set; }
    }
}

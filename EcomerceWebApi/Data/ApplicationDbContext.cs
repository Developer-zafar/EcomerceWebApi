using Microsoft.EntityFrameworkCore;

namespace EcommerceWebApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        
        public DbSet<Model.Category> Categories { get; set; }
        public DbSet<Model.Product> Products { get; set; }
        public DbSet<Model.User> Users { get; set; }
        public DbSet<Model.Order> Orders { get; set; }
        public DbSet<Model.OrderItem> OrderItems { get; set; }

    }
}
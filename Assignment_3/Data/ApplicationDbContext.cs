using Microsoft.EntityFrameworkCore;
using Assignment_3.Models;

namespace Assignment_3.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

    public DbSet<Product> Products { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<Order> Orders { get; set; }
    }
}
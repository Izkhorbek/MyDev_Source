using Microsoft.EntityFrameworkCore;
using Register.API.Models.Domain;

namespace Register.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }
        public DbSet<Product> Product { get; set; }     

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var product = new List<Product>
            {
                new Product
                {
                   Id = Guid.Parse("ad3b4d2f-5afc-4535-a353-4f6961d8c6f1"),
                   Name = "Futbolka",
                   Code = 25465
                },

                new Product
                {
                   Id = Guid.Parse("1deaec52-a945-4167-9e70-61ccddf9581e"),
                   Name = "Kuylak",
                   Code = 123445
                }
            };

            modelBuilder.Entity<Product>().HasData(product);
        }
    }
}

using LibCommon.HttpModels;
using Microsoft.EntityFrameworkCore;
using SignupLogin.API.Models;
using System.Data;

namespace SignupLogin.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Register> Users{ get; set; }

        public DbSet<UserCompanyInfo> UserCompanyInfos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Register>().ToTable("Users");
            modelBuilder.Entity<Register>().HasKey(primary => primary.username);

            modelBuilder.Entity<UserCompanyInfo>().ToTable("UserCompanyInfo");
            modelBuilder.Entity<UserCompanyInfo>().HasKey(primary => primary.id);

        }
    }
}

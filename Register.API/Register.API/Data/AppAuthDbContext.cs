using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Register.API.Models.Domain;
using Register.API.Models.DTO;

namespace Register.API.Data
{
    public class AppAuthDbContext : IdentityDbContext<MySqlRegisterRequestDomain>
    {
        public AppAuthDbContext(DbContextOptions<AppAuthDbContext> options) : base(options)
        {
        }

        DbSet<MySqlRegisterRequestDomain> mySqlRegisterRequestDomains { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var readerRoleId = "a74d69a3-80f0-412d-800f-ffffe985e087";
            var writerRoleId = "04661edf-103b-4d6b-8121-6a7906dc5505";

            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = readerRoleId,
                    ConcurrencyStamp = readerRoleId,
                    Name = "Reader",
                    NormalizedName = "Reader".ToUpper()
                },
                new IdentityRole
                {
                    Id = writerRoleId,
                    ConcurrencyStamp = writerRoleId,
                    Name = "Writer",
                    NormalizedName = "Writer".ToUpper()
                }
            };


            builder.Entity<IdentityRole>().HasData(roles);

        }
    }
}

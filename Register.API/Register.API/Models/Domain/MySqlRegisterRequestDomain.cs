using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Register.API.Models.Domain
{
    public class MySqlRegisterRequestDomain: IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string? CompanyName { get; set; }

        public string? WebAddress { get; set; }

        public string? Image { get; set; }
    }
}

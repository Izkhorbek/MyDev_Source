using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Register.API.Models.DTO
{
    public class RegisterRequestDto 
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }
       
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string? CompanyName { get; set; }

        public string? WebAddress { get; set; }

        public string? Image { get; set; }

        [Required]
        public string[] Roles { get; set; }
    }
}

using Microsoft.AspNetCore.Components.Forms;

namespace SignupLogin.API.Models
{
    public class Register : User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? CompanyName { get; set; }
        public string? WebAddress { get; set; }
        public string Image { get; set; }
    }
}

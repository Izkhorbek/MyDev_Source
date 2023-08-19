using LibCommon.HttpResponse;

namespace SignupLogin.API.Models
{
    public class ResponseRegister : Response
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string? CompanyName { get; set; }
        public string? WebAddress { get; set; }
        public string Image { get; set; }
    }
}

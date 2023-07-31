using System.ComponentModel.DataAnnotations;

namespace Register.API.Models.DTO
{
    public class LoginRequestDto
    {
        [Required]
        public string UserName { get; set; }
        
        [Required]
        [DataType(DataType.Password)]   
        public string Password { get; set; }
    }
}

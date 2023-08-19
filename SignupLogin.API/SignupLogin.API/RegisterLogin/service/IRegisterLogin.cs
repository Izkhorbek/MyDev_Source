using SignupLogin.API.Models;

namespace SignupLogin.API.RegisterLogin.service
{
    public interface IRegisterLogin
    {
        Task<ResponseLogin> Login(Login user);
        
        Task<ResponseRegister> Register(Register user);
    }
}

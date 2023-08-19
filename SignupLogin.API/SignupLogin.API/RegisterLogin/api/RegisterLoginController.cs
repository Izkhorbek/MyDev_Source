using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SignupLogin.API.Helper;
using SignupLogin.API.Models;
using SignupLogin.API.RegisterLogin.service;

namespace SignupLogin.API.RegisterLogin.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterLoginController : AppBaseController<IRegisterLogin>
    {
        private readonly IRegisterLogin registerLogin;

        public RegisterLoginController(IRegisterLogin registerLogin) :base (registerLogin)
        {
            this.registerLogin = registerLogin;
        }
        // Post Registration
        [HttpPost("registration")]
        public async Task<IActionResult> Register([FromBody] Register user)
        {
            ResponseRegister responseRegister = await registerLogin.Register(user);
            return MakeResponse(responseRegister, responseRegister.error_code);
        }

        // Post: login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login user)
        {
            ResponseLogin responseLogin = await registerLogin.Login(user); 
            return MakeResponse(responseLogin, responseLogin.error_code);   
        }
    }
}

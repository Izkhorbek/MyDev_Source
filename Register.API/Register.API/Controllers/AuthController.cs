using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Register.API.Models.Domain;
using Register.API.Models.DTO;
using Register.API.Repositories.Interface;

namespace Register.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<MySqlRegisterRequestDomain> userManager;
        private readonly ITokenRepository tokenRepository;

        public AuthController(UserManager<MySqlRegisterRequestDomain> userManager, ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }


        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            var identityUser = new MySqlRegisterRequestDomain
            {
                UserName = registerRequestDto.UserName,
                Email= registerRequestDto.EmailAddress,
                FirstName = registerRequestDto.FirstName,
                LastName = registerRequestDto.LastName,
                PhoneNumber = registerRequestDto.PhoneNumber,
                CompanyName = registerRequestDto.CompanyName,
                WebAddress = registerRequestDto.WebAddress,
                Image = registerRequestDto.Image
            };

            var identityResult = await userManager.CreateAsync(identityUser, registerRequestDto.Password);

            if (identityResult.Succeeded)
            {
                // Add Roles to this User
                if (registerRequestDto.Roles != null && registerRequestDto.Roles.Any())
                {
                    identityResult = await userManager.AddToRolesAsync(identityUser, registerRequestDto.Roles);

                    if (identityResult.Succeeded)
                    {
                        return Ok("User was registered!");
                    }
                }
            }

            return BadRequest("Something went wrong.");
        }


        [HttpPost]
        [Route("Login")]
    
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var user = await userManager.FindByNameAsync(loginRequestDto.UserName);

            if(user!=null)
            {
                var checkPasswordResult  = await userManager.CheckPasswordAsync(user, loginRequestDto.Password);
                
                if(checkPasswordResult)
                {
                    //Get Roles for this user
                    var roles = await userManager.GetRolesAsync(user);

                    if (roles != null)
                    {
                        //Create Token
                        var jwtToken = tokenRepository.CreateJWTToken(user, roles.ToList());
                        var response = new LoginResponseDto
                        {
                            JwtToken = jwtToken
                        };

                        return Ok(response);
                    }
                }
            }

            return BadRequest("Username or Password incorrect!");
        }
    }
}

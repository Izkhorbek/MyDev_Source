using Register.API.Models.Domain;

namespace Register.API.Repositories.Interface
{
    public interface ITokenRepository
    {
        string CreateJWTToken(MySqlRegisterRequestDomain user, List<string> roles);

    }
}

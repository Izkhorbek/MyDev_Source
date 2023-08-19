
using LibCommon.HttpModels;
using LibCommon.HttpResponse;

namespace SignupLogin.API.Models
{
    public class ResponseLogin: Response<User>
    {
        public UserCompanyInfo? companyInfo { get; set; } = null;
    }
}

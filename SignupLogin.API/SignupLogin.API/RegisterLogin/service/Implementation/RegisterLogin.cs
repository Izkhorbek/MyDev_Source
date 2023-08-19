using Microsoft.EntityFrameworkCore;
using SignupLogin.API.Data;
using SignupLogin.API.Models;
using SignupLogin.API.Helper;
using SignupLogin.API.AesOperationSystem;
using System.Net;
using LibCommon.HttpModels;

namespace SignupLogin.API.RegisterLogin.service.Implementation
{
    public class RegisterLogin :IRegisterLogin
    {
        private readonly AppDbContext appDbContext;

        public RegisterLogin(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<ResponseLogin> Login(Login user)
        {
            ResponseLogin? responseLogin = new ResponseLogin();

            var foundUser = await appDbContext.Users                
                .Where(item => item.username.Equals(user.UserName))
                .FirstOrDefaultAsync();

            if (foundUser == null)
            {
                responseLogin.data = null;
                responseLogin.message = Constants.DoNotExist;
                return responseLogin;
            }


            string decryptPassword = AesOperation.DecryptString(foundUser.password);
            if(!user.Password.Equals(decryptPassword)) 
            {
                responseLogin.result = false;
                responseLogin.message = Constants.WrongPassword;
                responseLogin.error_code = (int)HttpStatusCode.BadRequest;
                
                return responseLogin;
            }

            UserCompanyInfo? userCompanyInfo = await appDbContext.UserCompanyInfos
                    .Where(item => item.username.Equals(user.UserName))
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

            if(userCompanyInfo == null)
            {
                responseLogin.companyInfo = new UserCompanyInfo();
            }

            responseLogin.data.Copy(foundUser);
            responseLogin.result = true;
            responseLogin.message = Constants.Exist;
            responseLogin.error_code = (int)HttpStatusCode.OK;

            return responseLogin;
        }

        public async Task<ResponseRegister> Register(Register user)
        {
            ResponseRegister? responseRegister = new ResponseRegister();

            var foundUser = await appDbContext.Users
                .Where(item => item.username.Equals(user.username))
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if(foundUser != null) 
            {
                responseRegister.error_code = (int)HttpStatusCode.Found;
                responseRegister.message = Constants.Exist;

                return responseRegister;
            }

            var newUser = new Register
            {
                password = AesOperation.EncryptString(user.password),
                username = user.username,
                lan_id = user.lan_id,
                on_notification = user.on_notification,
                reg_date = user.reg_date,
                FirstName  = user.FirstName,
                LastName  = user.LastName,
                Email  = user.Email,
                PhoneNumber  = user.PhoneNumber,
                CompanyName = user.CompanyName,
                WebAddress = user.WebAddress,
                Image = user.Image
            };

            appDbContext.Users.Add(newUser);

            try
            {
                await appDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                responseRegister.result = false;
                responseRegister.message = ex.Message;

                return responseRegister;
            }

            responseRegister.result = true;
            responseRegister.message = Constants.Success;
            responseRegister.UserName = newUser.username;
            responseRegister.FirstName = newUser.FirstName;
            responseRegister.LastName  = newUser.LastName;  
            responseRegister.Email = newUser.Email;
            responseRegister.PhoneNumber = newUser.PhoneNumber;
            responseRegister.Password = newUser.password;
            responseRegister. CompanyName = newUser.CompanyName;
            responseRegister. WebAddress = newUser.WebAddress;
            responseRegister.Image = newUser.Image;
            responseRegister.error_code = (int)HttpStatusCode.OK;

            return responseRegister;
        }
    }
}

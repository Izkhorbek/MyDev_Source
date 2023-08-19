using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace SignupLogin.API.Helper
{
    public class AppBaseController<IService> : ControllerBase
    {
        private readonly IService service;

        public AppBaseController(IService service)
        {
            this.service = service;
        }

        protected IActionResult MakeResponse(object obj, int error_code)
        {
            switch (error_code)
            {
                case (int)HttpStatusCode.NotFound: return NotFound(obj);
                case (int)HttpStatusCode.BadRequest: return BadRequest(obj);
            }

            return Ok(obj);
        }
    }
}

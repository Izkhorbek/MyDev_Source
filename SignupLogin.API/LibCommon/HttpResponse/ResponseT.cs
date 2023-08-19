using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LibCommon.HttpResponse
{
    public class Response <ResponseData> where ResponseData : class, new()
    {
        public bool result { get; set; }
        public string? message { get; set; }
        public int error_code { get; set; }
        public ResponseData? data { get; set; }

        public Response()
        {
            result = false;
            message = Constants.NotFound;
            error_code = (int)HttpStatusCode.NotFound;

            data = new ResponseData();
        }
    }

    public class Response
    {
        public bool result { get; set; }
        public String message { get; set; }
        public int error_code { get; set; }

        public Response()
        {
            result = false;
            message = Constants.NotFound;
            error_code = (int)HttpStatusCode.NotFound;
        }
    }
}

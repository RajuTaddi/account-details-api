using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace account.details.common.exceptions
{
    public class ApiException : Exception
    {
        public HttpStatusCode Code { get; set; } = HttpStatusCode.InternalServerError;
        public string Description { get; set; }
        public object Details { get; set; }

        public ApiException(int? code) : base()
        {
            if(code != null)
                this.Code = (HttpStatusCode)code;
        }

        public ApiException(string message, int? code): base(message)
        {
            if (code != null)
                this.Code = (HttpStatusCode)code;
        }
    }
}

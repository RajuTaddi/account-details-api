using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace account.details.common.exceptions
{
    public class DataNotFoundException : ApiException
    {
        public DataNotFoundException() 
            : base((int)HttpStatusCode.NotFound)
        { }
        
        public DataNotFoundException(string message):
            base(message, (int)HttpStatusCode.NotFound)
        { } 
    }
}

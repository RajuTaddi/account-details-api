using System;
using System.Collections.Generic;
using System.Text;

namespace account.details.common.models
{
    public class Error
    {
        public string Code { get; set; }
        public object ErrorDetails { get; set; }
    }
}

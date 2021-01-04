using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Application.Exeptions
{
    public class RestException : Exception
    {
        public HttpStatusCode Code { get; }
        public object Errors { get; }
        public RestException(HttpStatusCode code, object err)
        {
            Code = code;
            Errors = err;
        }
    }
}

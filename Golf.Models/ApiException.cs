using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;

namespace Golf.Tournament.Models
{
    public class ApiException : Exception
    {
        public ApiException(HttpResponseMessage response)
            : base(response.ReasonPhrase)
        {
            this.StatusCode = response.StatusCode;
            this.Response = response;
        }

        public HttpResponseMessage Response { get; private set; }
        public HttpStatusCode StatusCode { get; set; }
    }
}
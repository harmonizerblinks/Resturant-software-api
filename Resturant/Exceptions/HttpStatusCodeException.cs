using System;
using Newtonsoft.Json.Linq;

namespace Resturant.Exceptions
{
    public class HttpStatusCodeException : Exception
    {
        public HttpStatusCodeException(int statusCode)
        {
            StatusCode = statusCode;
        }

        public HttpStatusCodeException(int statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }

        public HttpStatusCodeException(int statusCode, Exception inner) : this(statusCode)
        {
        }

        public HttpStatusCodeException(int statusCode, JObject errorObject) : this(statusCode)
        {
            ContentType = @"application/json";
        }

        public int StatusCode { get; set; }

        public string ContentType { get; set; } = @"text/plain";
    }
}
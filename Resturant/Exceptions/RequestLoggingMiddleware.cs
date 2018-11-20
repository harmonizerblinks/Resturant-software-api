using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resturant.Exceptions
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware( RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        //public async Task Invoke(HttpContext context)
        //{
        //    await _next.Invoke(context);
        //}

        public async Task Invoke(HttpContext context)
        {
            var injectedRequestStream = new MemoryStream();

            try
            {
                var requestLog =
                $"REQUEST HttpMethod: {context.Request.Method}, Path: {context.Request.Path}";

                using (var bodyReader = new StreamReader(context.Request.Body))
                {
                    var bodyAsText = bodyReader.ReadToEnd();
                    if (string.IsNullOrWhiteSpace(bodyAsText) == false)
                    {
                        requestLog += $", Body : {bodyAsText}";
                    }

                    var bytesToWrite = Encoding.UTF8.GetBytes(bodyAsText);
                    injectedRequestStream.Write(bytesToWrite, 0, bytesToWrite.Length);
                    injectedRequestStream.Seek(0, SeekOrigin.Begin);
                    context.Request.Body = injectedRequestStream;
                }

                Console.WriteLine(requestLog);
                _logger.LogTrace(requestLog);

                await _next.Invoke(context);
            }
            finally
            {
                injectedRequestStream.Dispose();
            }
        }

    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Resturant.Exceptions
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            
            var injectedRequestStream = new MemoryStream();
            var requestLog = $"REQUEST HttpMethod: {context.Request.Method}, Path: {context.Request.Path}";

            using (var bodyReader = new StreamReader(context.Request.Body))
            {
                var bodyAsText = bodyReader.ReadToEnd();
                if (string.IsNullOrWhiteSpace(bodyAsText) == false)
                {
                    requestLog += $", Body : {bodyAsText}";
                }
                //Format the response from the server
                var request = await FormatRequest(context.Request, bodyAsText);

                //TODO: Save log to chosen datastore
                Console.WriteLine(request);

                var bytesToWrite = Encoding.UTF8.GetBytes(bodyAsText);
                injectedRequestStream.Write(bytesToWrite, 0, bytesToWrite.Length);
                injectedRequestStream.Seek(0, SeekOrigin.Begin);
                context.Request.Body = injectedRequestStream;
            }

            //Copy a pointer to the original response body stream
            var originalBodyStream = context.Response.Body;

            if (context.User.Identity.IsAuthenticated)
            {
                var response = context.User.Claims;
                Console.WriteLine(response);
                
            }

            //Create a new memory stream...
            using (var responseBody = new MemoryStream())
            {
                //...and use that for the temporary response body
                context.Response.Body = responseBody;

                //Continue down the Middleware pipeline, eventually returning to this class
                await _next(context);

                //Format the response from the server
                var response = await FormatResponse(context.Response);

                //TODO: Save log to chosen datastore
                Console.WriteLine(response);
                _logger.LogTrace(requestLog);

                //Copy the contents of the new memory stream (which contains the response) to the original stream, which is then returned to the client.
                await responseBody.CopyToAsync(originalBodyStream);
            }

        }


        private async Task<string> FormatRequest(HttpRequest request, string data)
        {
            var body = data;
            Console.WriteLine(body);

            return $"{request.Scheme} {request.Host}{request.Path} {request.QueryString} {data}";
        }

        //private async Task<string> FormatRequest(HttpRequest request)
        //{
        //    request.EnableRewind();

        //    var buffer = new byte[Convert.ToInt32(request.ContentLength)];

        //    await request.Body.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false);

        //    var bodyAsText = Encoding.UTF8.GetString(buffer);

        //    request.Body.Position = 0;

        //    return $"{Environment.NewLine}{Environment.NewLine}{ApiRequest}{Environment.NewLine}{request.Scheme} {request.Host}{request.Path} {request.QueryString} {bodyAsText}{Environment.NewLine}{Environment.NewLine}";
        //}

        private async Task<string> FormatResponse(HttpResponse response)
        {
            //We need to read the response stream from the beginning...
            response.Body.Seek(0, SeekOrigin.Begin);

            //...and copy it into a string
            string text = await new StreamReader(response.Body).ReadToEndAsync();

            //We need to reset the reader for the response so that the client can read it.
            response.Body.Seek(0, SeekOrigin.Begin);

            //Return the string for the response, including the status code (e.g. 200, 404, 401, etc.)
            return $"{response.StatusCode}: {text}";
        }
    }
}

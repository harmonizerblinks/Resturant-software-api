using Microsoft.AspNetCore.Builder;

namespace Resturant.Exceptions
{
    public static class HttpStatusCodeExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseHttpStatusCodeExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<HttpStatusCodeExceptionMiddleware>();
        }

        //public static IApplicationBuilder UseLoggingMiddleware(this IApplicationBuilder builder)
        //{
        //    return builder.UseMiddleware<LoggingMiddleware>();
        //}


        //public static IApplicationBuilder RequestLoggingMiddleware(this IApplicationBuilder builder)
        //{
        //    return builder.UseMiddleware<RequestLoggingMiddleware>();
        //}
    }
}
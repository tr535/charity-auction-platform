using Serilog;
using System.Net;

namespace server.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // התיעוד ללוג (Serilog) שתופס הכל
                Log.Error(ex, "שגיאה נתפסה במידלוואר בנתיב: {Path}", context.Request.Path);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                await context.Response.WriteAsJsonAsync(new
                {
                    error = "Internal Server Error",
                    message = ex.Message
                });
            }
        }
    }
}
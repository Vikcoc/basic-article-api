using Microsoft.AspNetCore.Diagnostics;

namespace basic_article_api.ApplicationExceptions
{
    public class LittleExceptionHandler : IExceptionHandler
    {
        //https://youtu.be/uOEDM0c9BNI?t=569&si=ZHZ2Y-AsfXkeugYX
        //https://learn.microsoft.com/en-us/aspnet/core/fundamentals/error-handling?view=aspnetcore-8.0#iexceptionhandler

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is not ApplicationException)
                return false; //it is automatically logged


            httpContext.Response.StatusCode = exception switch 
            {
                BadRequestException => 400,
                ServiceUnavailableException => 503,
                _ => 500 // if i'm throwing something that i don't declare here it's a problem
            };

            await httpContext.Response.WriteAsJsonAsync(exception.Message, cancellationToken);
            return true;
        }
    }
}

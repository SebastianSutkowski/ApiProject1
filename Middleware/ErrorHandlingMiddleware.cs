using Microsoft.AspNetCore.Http;
using _ApiProject1_.Exceptions;

namespace _ApiProject1_.Middleware
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger)
        {
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            
            try
            {
                await next.Invoke(context);
            }
            catch (BadRequestException barRequest)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync(barRequest.Message);
            }
            catch (ForbidException)
            {
                context.Response.StatusCode = 403;
               
            }
            catch(NotFoundException notFound)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync($"{notFound.Message}");
            }
            catch(Exception e)
            {
                _logger.LogError(e, e.Message);
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("Something went wrong");

            }
        }
    }
}

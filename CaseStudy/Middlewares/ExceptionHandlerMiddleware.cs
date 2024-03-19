using CaseStudy.EFCore.Exceptions;

namespace CaseStudy.Middlewares
{
    public class ExceptionHandlerMiddleware(RequestDelegate Next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await Next.Invoke(context);
            }
            catch (EntityNotFoundException)
            {
                context.Response.StatusCode = StatusCode.NotFound;
            }
        }
    }
}

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.ApplicationProgrammingInterface.PersonaCliente.Middleware
{
    public class GoblalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var details = new ProblemDetails()
            {
                Instance = httpContext.Request.Path,
                Status = httpContext.Response.StatusCode,
                Title = exception.Message,
                Detail = exception.StackTrace

            };
            await httpContext.Response.WriteAsJsonAsync(details);
            return true;

        }
    }
}

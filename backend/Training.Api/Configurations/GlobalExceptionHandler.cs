using Microsoft.AspNetCore.Diagnostics;
using Training.Api.Models.Responses.Base;
using Training.CustomException;

namespace Training.Api.Configurations
{
    public class GlobalExceptionHandler() : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext,
            Exception exception, CancellationToken cancellationToken)
        {
            var response = new ResultRes<string>
            {
                Success = false,
                Error = exception.Message
            };

            httpContext.Response.StatusCode = exception is CustomHttpException customException ? customException.code : 500;
            await httpContext.Response.WriteAsJsonAsync(response);

            return true;
        }
    }
}

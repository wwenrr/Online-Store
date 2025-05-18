using Microsoft.Extensions.DependencyInjection;
using Training.BusinessLogic.Services;
using Training.CustomException;

namespace Training.Api.Middleware
{
    public class GlobalMiddleware(RequestDelegate _next, IServiceProvider serviceScopeFactory)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/api"))
            {
                using (var scope = serviceScopeFactory.CreateScope())
                {
                    var middlewareService = scope.ServiceProvider.GetRequiredService<IMiddlewareService>();

                    var resDTO = await middlewareService.HandleApiMiddleware(context);

                    context.Items["email"] = resDTO.Email;
                    context.Items["role"] = resDTO.Role;
                }
            }

            if (context.Request.Path.StartsWithSegments("/admin"))
            {
                var adminKey = context.Request.Headers["ADMIN_KEY"].ToString();

                if(!adminKey.Equals(Environment.GetEnvironmentVariable("ADMIN_KEY")))
                {
                    throw new CustomHttpException("Sai admin token", 403);
                }
            }

            if (context.Request.Path.StartsWithSegments("/api/clerk"))
            {
                if ((int)context.Items["role"] != 2)
                {
                    throw new CustomHttpException("Role không phù hợp", 403);
                }
            }

            await _next(context);
        }
    }
}

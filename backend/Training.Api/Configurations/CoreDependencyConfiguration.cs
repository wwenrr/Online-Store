using Training.Api.Middleware;
using Training.BusinessLogic.Services;
using Training.Repository.UoW;

namespace Training.Api.Configurations
{
    public static class CoreDependencyConfiguration
    {
        public static void AddCoreDependencies(this IServiceCollection collection, IConfiguration configuration)
        {
            collection.AddHttpContextAccessor();
            collection.AddServices();
            collection.AddUnitOfWork(configuration);
        }

        private static void AddServices(this IServiceCollection collection)
        {
            collection.AddScoped<IExampleService, ExampleService>();
            collection.AddScoped<IAuthService, AuthService>();
            collection.AddScoped<IMiddlewareService, MiddlewareService>();
            collection.AddScoped<IApiService, ApiService>();
            collection.AddScoped<IAdminService, AdminService>();
            collection.AddScoped<IClerkService, ClerkService>();
        }
    }
}

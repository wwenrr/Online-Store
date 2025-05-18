using Training.BusinessLogic.Services;
using Training.Repository.UoW;
using AutoMapper;

namespace Training.Cms.Configurations
{
    public static class CoreDependencyConfiguration
    {
        public static void AddCoreDependencies(this IServiceCollection collection, IConfiguration configuration)
        {
            collection.AddHttpContextAccessor();
            collection.AddServices();
            collection.AddUnitOfWork(configuration);
            collection.AddAutoMapper(typeof(Program).Assembly);
        }

        private static void AddServices(this IServiceCollection collection)
        {
            collection.AddScoped<IAdminService, AdminService>();
            collection.AddScoped<IApiService, ApiService>();
        }
    }
}

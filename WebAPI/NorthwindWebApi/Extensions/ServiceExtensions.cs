using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Northwind.Contracts;
using Northwind.LoggerService;
using Northwind.Entities.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Northwind.Repository;
using Northwind.Contracts;

namespace NorthwindWebApi.Extenstions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection service) =>
            service.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
            });

        // add IIS configure options deploy to ISS
        public static void ConfigureIISIntegration(this IServiceCollection service) =>
            service.Configure<IISOptions>(options =>
            {

            });

        //create a service once per request
        public static void ConfigureLoggerService(this IServiceCollection service) =>
            service.AddScoped<ILoggerManager, LoggerManager>();

        //config to db
        public static void ConfigureDbContext(this IServiceCollection service, IConfiguration configuration) =>
            service.AddDbContext<RepositoryContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("development")
            ));

        public static void ConfigureRepositoryManager(this IServiceCollection service) =>
            service.AddScoped<IRepositoryManager, RepositoryManager>();

        public static void ConfigureServiceManager(this IServiceCollection service) =>
            service.AddScoped<IServiceManager, ServiceManager>();
    }
}

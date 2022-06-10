using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Northwind.Contracts;
using Northwind.LoggerService;
using Northwind.Entities.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Northwind.Repository;
using Northwind.Contracts;
using Microsoft.AspNetCore.Identity;
using Northwind.Entities.Models;
using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

        public static void ConfigureIdentity(this IServiceCollection service)
        {
            var builder = service.AddIdentityCore<User>(o =>
            {
                o.Password.RequireDigit = true;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 10;
                o.User.RequireUniqueEmail = true;
            });

            builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), builder.Services);
            builder.AddEntityFrameworkStores<RepositoryContext>()
                .AddDefaultTokenProviders();
        }

        public static void ConfigureJWT(this IServiceCollection service, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");
            var secretKey = Environment.GetEnvironmentVariable("SECRET");
            service.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.GetSection("validIssuer").Value,
                    ValidAudience = jwtSettings.GetSection("validAudience").Value,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };
            });
        }

        public static void ConfigureAuthenticationManager(this IServiceCollection services) =>
            services.AddScoped<IAuthenticationManager, AuthenticationManager>();



    }
}

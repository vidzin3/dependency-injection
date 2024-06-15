using Contract;
using Entities.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Repositorys;
using Service.Contract;
using Services;
using System.Text;

namespace DependencyInjection.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureServiceCollection(this IServiceCollection services)
        {
            services.AddScoped<IAccountInterface, AccountRepository>();
            services.AddScoped<IServiceManager, ServiceManager>();
        }
        public static void ConfigureRepositoryManager(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryManager, RepositoryManager>();
        }
        public static void ConfigurePostgresContext(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbContext<RepositoryContext>(option =>
            {
                option.UseNpgsql(configuration.GetConnectionString("D2Connection"));
            });
        }
        public static void ConfigureServiceCors(this IServiceCollection services)
        {
            services.AddCors(t =>
            {
                t.AddDefaultPolicy(option =>
                {
                    option.AllowAnyHeader();
                    option.AllowAnyMethod();
                    option.AllowAnyOrigin();
                });
            });
        }
        public static void ConfigureJwtAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(t =>
            {
                t.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                t.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                t.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(jwt =>
            {
                jwt.RequireHttpsMetadata = false;
                jwt.SaveToken = true;
                jwt.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = AuthContext.Issure,
                    ValidAudience = AuthContext.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AuthContext.Secret)),

                    ClockSkew = TimeSpan.Zero,
                    ValidateIssuerSigningKey = true
                };
            });
        }
    }
}

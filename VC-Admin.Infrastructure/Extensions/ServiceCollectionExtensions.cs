using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VC_Admin.Application.Interfaces.Repository;
using VC_Admin.Application.Interfaces.Services;
using VC_Admin.Application.Services;
using VC_Admin.Infrastructure.Contexts;
using VC_Admin.Infrastructure.Repositories;

namespace VC_Admin.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(opts =>
                opts.UseNpgsql(configuration.GetConnectionString("master")));

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }

        public static IServiceCollection AddScopedServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAuthService, AuthService>();

            return services;
        }
    }
}

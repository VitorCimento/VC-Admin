using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VC_Admin.Application.Interfaces.Repository;
using VC_Admin.Application.Interfaces.Services;
using VC_Admin.Application.Services;
using VC_Admin.Infrastructure.Contexts;
using VC_Admin.Infrastructure.Repositories;
using System.Text;
using Microsoft.OpenApi.Models;

namespace VC_Admin.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(opts =>
                opts.UseNpgsql(configuration.GetConnectionString("master")));

            return services;
        }

        public static IServiceCollection ConfigureRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }

        public static IServiceCollection ConfigureScopedServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAuthService, AuthService>();

            return services;
        }

        public static IServiceCollection ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var secret = configuration["Jwt:Secret"] ?? throw new InvalidOperationException("Jwt:Secret não foi definido!");
            var key = Encoding.UTF8.GetBytes(secret);

            services
                .AddAuthentication(opts =>
                {
                    opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(opts =>
                {
                    opts.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            if (context.Request.Cookies.TryGetValue("jwt", out var cookieToken))
                                context.Token = cookieToken;

                            return Task.CompletedTask;
                        }
                    };

                    opts.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    };

                    opts.RequireHttpsMetadata = Convert.ToBoolean(configuration["Jwt:RequireHttpsMetadata"]);
                    //opts.SaveToken = true;
                });



            return services;
        }

        public static IServiceCollection ConfigureAuthorization(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthorization(opts =>
            {
                opts.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
            });

            return services;
        }

        public static IServiceCollection ConfigureSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(opts =>
            {
                var scheme = new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    },
                    Description = "Use 'Bearer {token}' ou teste com cookie 'jwt'.",
                    BearerFormat = "JWT",
                    Scheme = "bearer",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                };

                opts.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "API VC-Admin", Version = "v1" });
                opts.AddSecurityDefinition(scheme.Reference.Id, scheme);
                opts.AddSecurityRequirement(new OpenApiSecurityRequirement 
                {
                    { scheme, Array.Empty<string>() }
                });
            });

            return services;
        }
    }
}

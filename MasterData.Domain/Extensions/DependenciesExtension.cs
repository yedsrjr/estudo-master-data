using Domain.Repository;
using JJMasterData.Commons.Configuration;
using MasterData.Domain.Repository;
using MasterData.Domain.Services;
using MasterData.Domain.Services.API;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace MasterData.Domain.Extensions
{
    public static class DependenciesExtension
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<DashboardRepository>();
            services.AddScoped<OrderValidate>();
            services.AddScoped<OrderRepository>();
            services.AddScoped<CustomerRepository>();
            services.AddScoped<BaseRepository>();
        }
        public static void AddServices(this IServiceCollection services)
        {
            services.AddTransient<ClienteService>();
            services.AddTransient<ProdutoService>();
            services.AddTransient<PedidoService>();
            services.AddTransient<PedidoItemService>();
            services.AddTransient<PriceCustomerService>();
            services.AddTransient<LogOrderService>();
            services.AddTransient<DashboardService>();
        }
        public static void AddRepositoriesApi(this IServiceCollection services)
        {
            services.AddScoped<CustomerRepository>();
            services.AddScoped<ProductRepository>();
            services.AddScoped<PriceCustomerRepository>();
            services.AddScoped<OrderRepository>();
        }
        public static void AddServicesApi(this IServiceCollection services)
        {
            services.AddTransient<CustomerApiService>();
            services.AddTransient<ProductApiService>();
            services.AddTransient<PriceCustomerApiService>();
            services.AddTransient<OrderApiService>();
            services.AddTransient<FileUploadService>();
            services.AddTransient<TokenService>();
        }
        public static void AddApiInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = JwtConfiguration.GetSecurityKey(configuration),
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = false,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.FromMinutes(5)
                };

                x.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = ctx =>
                    {
                        var logger = ctx.HttpContext.RequestServices
                            .GetRequiredService<ILogger<Program>>();
                        logger.LogError(ctx.Exception, "Falha de autenticação: token inválido");
                        return Task.CompletedTask;
                    },
                    OnChallenge = ctx =>
                    {
                        var logger = ctx.HttpContext.RequestServices
                            .GetRequiredService<ILogger<Program>>();
                        logger.LogWarning("Desafio de autenticação: token ausente ou inválido");
                        return Task.CompletedTask;
                    },
                    OnForbidden = ctx =>
                    {
                        var logger = ctx.HttpContext.RequestServices
                            .GetRequiredService<ILogger<Program>>();
                        logger.LogWarning("Acesso negado: usuário não autorizado para este recurso");
                        return Task.CompletedTask;
                    }
                };

            });

            services.AddAuthorization(x =>
            {
                x.AddPolicy("OnlyUser1", policy =>
                {
                    policy.RequireClaim("Id", "1");
                });

                x.AddPolicy("OnlyEdsonEmail", policy =>
                {
                    policy.RequireClaim(ClaimTypes.Name, "edson@email.com");
                });

                x.AddPolicy("Developers", policy =>
                {
                    policy.RequireClaim(
                        ClaimTypes.Name,
                        "edson@email.com",
                        "joao@email.com",
                        "maria@email.com");
                });
                
                x.AddPolicy("MasterAdmin", policy =>
                {
                    policy.RequireRole("admin");
                    policy.RequireClaim("Id", "1");
                    policy.RequireClaim(ClaimTypes.Name, "edson@email.com");
                });

                x.AddPolicy("InternalUsers", policy =>
                {
                    policy.RequireAssertion(context =>
                    {
                        var email = context.User.Identity?.Name;

                        return email?.EndsWith("@danone.com") == true;
                    });
                });

                x.AddPolicy("AdminOrManager", policy =>
                {
                    policy.RequireRole("admin", "manager");
                });

                x.AddPolicy("OnlyIT", policy =>
                {
                    policy.RequireClaim("Department", "IT");
                });

                x.AddPolicy("OnlyBuyers", policy =>
                {
                    policy.RequireClaim("Department", "Buyer");
                });
            });

            services.AddMemoryCache();
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddJJMasterDataCommons(configuration);
        }
    }
}

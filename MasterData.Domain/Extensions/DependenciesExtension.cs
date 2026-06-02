using Domain.Repository;
using JJMasterData.Commons.Configuration;
using MasterData.Domain.Repository;
using MasterData.Domain.Services;
using MasterData.Domain.Services.API;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.PrivateKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddAuthorization(x =>
            {
                x.AddPolicy("admin", p => p.RequireRole("admin"));
            });
            services.AddMemoryCache();
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddJJMasterDataCommons(configuration);
        }
    }
}

using Domain.Repository;
using JJMasterData.Commons.Configuration;
using MasterData.Domain.Repository;
using MasterData.Domain.Services;
using MasterData.Domain.Services.API;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MasterData.Domain.Extensions
{
    public static class DependenciesExtension
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<DashboardRepository>();
            services.AddScoped<OrderValidate>();
            services.AddScoped<PedidoRepository>();
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
        }
        public static void AddServicesApi(this IServiceCollection services)
        {
            services.AddTransient<CustomerApiService>();
            services.AddTransient<ProductApiService>();
            services.AddTransient<PriceCustomerApiService>();
        }
        public static void AddApiInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMemoryCache();
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddJJMasterDataCommons(configuration);
        }
    }
}

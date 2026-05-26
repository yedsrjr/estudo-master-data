using Domain.Repository;
using Domain.Services;
using MasterData.Domain.Repository;
using MasterData.Domain.Services;
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
    }
}

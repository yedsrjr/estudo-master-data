using Domain.Models.ViewModels;
using Domain.Repository;
using Microsoft.Extensions.Caching.Memory;

namespace MasterData.Domain.Services
{
    public class DashboardService(DashboardRepository repository, IMemoryCache cache)
    {
        public const string DashboardCountsPrefix = "Dashboard:Counts";
        public const string DashboardVersionKey = "Dashboard:Version";

        public async Task<DashboardViewModel> GetValuesDashboard(DateTime? de = null, DateTime? ate = null)
        {
            var version = cache.GetOrCreate(DashboardVersionKey, e => 1);
            var key = $"{DashboardCountsPrefix}:v{version}:{de:yyyyMMdd}:{ate:yyyyMMdd}";

            return await cache.GetOrCreateAsync(key, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);

                var vm = new DashboardViewModel
                {
                    ClientCount = repository.GetCountRecord("Clientes"),
                    OrderCount = repository.GetCountRecord("Pedidos"),
                    ProductCount = repository.GetCountRecord("Produtos"),
                    OrdersByStatus = repository.GetOrdersByStatus(de, ate)
                };

                return Task.FromResult(vm);
            }) ?? new DashboardViewModel();
        }

        public void InvalidateDashboardCache()
        {
            var current = cache.Get<int?>(DashboardVersionKey) ?? 1;
            cache.Set(DashboardVersionKey, current + 1);
        }
    }
}

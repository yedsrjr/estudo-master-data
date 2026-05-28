using MasterData.Domain.Models.DTOs.PriceCustomer;
using MasterData.Domain.Models.DTOs.Product;
using MasterData.Domain.Repository;

namespace MasterData.Domain.Services.API
{
    public class PriceCustomerApiService(PriceCustomerRepository repository)
    {
        public async Task<int> CountPricesAsync()
        {
            var total = await repository.CountAsync(repository.CountPrices());
            return total;
        }
        public async Task<List<PriceCustomerResponse>> GetPricesAsync(int page, int pageSize)
        {
            var cmd = repository.GetPrices(page, pageSize);
            var result = await repository.GetAsync<PriceCustomerResponse>(cmd);

            return result;
        }
        public async Task<List<PriceCustomerResponse?>> GetPriceById(int id)
        {
            var cmd = repository.GetPriceById(id);
            var result = await repository.GetAsync<PriceCustomerResponse>(cmd);

            return result;
        }
        public async Task<int> AddAsync(PriceItemRequest request)
        {
            var cmd = repository.AddPrice(request);
            var id = await repository.SetAsync(cmd);

            return id;
        }
    }
}

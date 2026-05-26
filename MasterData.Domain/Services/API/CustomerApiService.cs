using MasterData.API.Models.DTOs.Customer;
using MasterData.Domain.Models.DTOs;
using MasterData.Domain.Models.DTOs.Customer;
using MasterData.Domain.Models.ViewModels;
using MasterData.Domain.Repository;

namespace MasterData.Domain.Services.API
{
    public class CustomerApiService(CustomerRepository customerRepository)
    {
        public async Task<PagedResult<CustomerResponse>> GetCustomersAsync(int page, int pageSize)
        {
            var total = await customerRepository.CountAsync(customerRepository.GetCommandCustomerCount());
            
            var customers = await customerRepository.GetAsync<CustomerResponse>(
                customerRepository.GetCommandCustomer(page, pageSize));

            return new PagedResult<CustomerResponse>
            {
                Total = total,
                Page = page,
                PageSize = pageSize,
                Items = customers
            };
        }
        public async Task<CustomerResponse?> GetByIdAsync(int id)
        {
            var cmd = customerRepository.GetCustomerById(id);
            var customer = await customerRepository.GetAsync<CustomerResponse>(cmd);

            return customer.FirstOrDefault();
        }

        public async Task<int> AddAsync(CustomerRequest request)
        {
            var cmd = customerRepository.InsertCustomer(request);
            var id = await customerRepository.SetAsync(cmd);
            return Convert.ToInt32(id); 
        }
    }
}

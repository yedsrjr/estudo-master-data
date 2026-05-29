using MasterData.API.Models.DTOs.Customer;
using MasterData.Domain.Models.DTOs;
using MasterData.Domain.Models.DTOs.Customer;
using MasterData.Domain.Models.DTOs.Product;
using MasterData.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace MasterData.Domain.Services.API
{
    public class ProductApiService(ProductRepository repository)
    {
        public async Task<PagedResult<ProductResponse>> GetProducts(int page, int pageSize)
        {
            var total = await repository.CountAsync(repository.Count());

            var products = await repository.GetListAsync<ProductResponse>(
                repository.GetProducts(page, pageSize));

            return new PagedResult<ProductResponse>
            {
                Total = total,
                Page = page,
                PageSize = pageSize,
                Items = products
            };
        }

        public async Task<ProductResponse?> GetProductById(int id)
        {
            var product = repository.GetProductById(id);
            var result = await repository.GetListAsync<ProductResponse>(product);

            return result.FirstOrDefault();
        }
        public async Task<int> AddAsync(ProductRequest request)
        {
            var cmd = repository.AddProduct(request);
            var id = await repository.SetAsync(cmd);
            return id;
        }
        public async Task<int> UpdateAsync(int id, ProductRequest request)
        {
            var cmd = repository.UpdateProduct(id, request);
            var result = await repository.SetAsync(cmd);
            return id;
        }
        public async Task<bool> CancelProductAsync(int id)
        {
            var cmd = repository.CancelProduct(id);
            var result = await repository.CancelAsync(cmd);
            return Convert.ToInt32(result) > 0;
        }

    }
}

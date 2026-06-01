using Domain.Models.Enums;
using Domain.Repository;
using MasterData.Domain.Models.DTOs;
using MasterData.Domain.Models.DTOs.Order;
using MasterData.Domain.Repository;

namespace MasterData.Domain.Services.API
{
    public class OrderApiService(OrderRepository repository, FileUploadService fileService)
    {
        public async Task<PagedResult<OrderResponse>> GetOrdersAsync(int page, int pageSize, string? status)
        {
            int? intStatus = 0;

            if (!string.IsNullOrEmpty(status))
            {
                switch (status.ToLower())
                {
                    case "elaboration":
                        intStatus = (int)OrderStatus.Elaboration;
                        break;
                    case "send":
                        intStatus = (int)OrderStatus.Send;
                        break;
                    case "canceled":
                        intStatus = (int)OrderStatus.Canceled;
                        break;
                    default:
                        intStatus = null;
                        break;
                }
            }
            else
            {
                intStatus = null;
            }

            var total = await repository.CountAsync(repository.CountOrderByStatus(intStatus));
            var cmd = repository.GetAllOrders(page, pageSize, intStatus);
            var orders = await repository.GetListAsync<OrderResponse>(cmd);

            return new PagedResult<OrderResponse>
            {
                Total = total,
                Page = page,
                PageSize = pageSize,
                Items = orders
            };
        }

        public async Task<OrderResponse?> GetOrderByIdAsync(int id)
        {
            var order = await repository.GetListAsync<OrderResponse>(repository.GetOrderById(id));

            if (order == null) return null;

            return order.FirstOrDefault();
        }

        public async Task<OrderWithItensResponse?> GetOrderItemsAsync(int id)
        {
            var cmd = repository.GetOrderById(id);

            var order = await repository.GetAsync<OrderResponse>(cmd);

            if (order == null) return null;

            var itens = await repository.GetListAsync<ItemResponse>(repository.GetOrderItems(id));

            return new OrderWithItensResponse
            {
                Order = order,
                Itens = itens.Any() ? itens : null
            };
        }

        public async Task<int> PostOrderAsync(OrderRequest model)
        {
            var base64 = fileService.EnsureMimePrefix(model.Document!);

            var fileName = $"{Guid.NewGuid()}.{fileService.GetExtension(base64)}";
            model.Document = fileName;

            var cmd = repository.AddOrder(model);
            var id = await repository.SetAsync(cmd);

            await fileService.SaveAsync(base64, id, fileName);

            return id;
        }
        public async Task<bool> SendOrderAsync(int id)
        {
            var cmd = repository.SendOrder(id);
            var result = await repository.SetAsync(cmd);
            return Convert.ToInt32(result) > 0;
        }
        public async Task<bool> CancelOrderAsync(int id)
        {
            var cmd = repository.CancelOrder(id);
            var result = await repository.CancelAsync(cmd);
            return Convert.ToInt32(result) > 0;
        }
    }
}

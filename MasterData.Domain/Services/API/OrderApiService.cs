using Domain.Models.Enums;
using Domain.Repository;
using MasterData.Domain.Models.DTOs;
using MasterData.Domain.Models.DTOs.Order;

namespace MasterData.Domain.Services.API
{
    public class OrderApiService(OrderRepository repository)
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
            var orders = await repository.GetAsync<OrderResponse>(cmd);

            return new PagedResult<OrderResponse>
            {
                Total = total,
                Page = page,
                PageSize = pageSize,
                Items = orders
            };
        }
    }
}

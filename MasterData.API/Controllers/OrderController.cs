using MasterData.Domain.Models.DTOs;
using MasterData.Domain.Models.DTOs.Order;
using MasterData.Domain.Models.DTOs.Product;
using MasterData.Domain.Models.ViewModels;
using MasterData.Domain.Services.API;
using Microsoft.AspNetCore.Mvc;

namespace MasterData.API.Controllers
{
    [ApiController]
    public class OrderController(OrderApiService service) : ControllerBase
    {
        [HttpGet("v1/orders")]
        public async Task<IActionResult> GetAsync([FromQuery] int page = 1, [FromQuery] int pageSize = 20, 
            [FromQuery] string status = null)
        {
            try
            {
                var orders = await service.GetOrdersAsync(page, pageSize, status);
                return Ok(new ResultViewModel<PagedResult<OrderResponse>>(orders));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<PagedResult<OrderResponse>>("05X02 - Falha interna no servidor"));
            }
        }

        //[HttpGet("v1/orders/{id:int}")]
        //public async Task<IActionResult> GetOrderWithItensAsync([FromQuery] string status = null)
        //{
        //    try
        //    {
        //        var orders = await service.GetOrdersAsync(page, pageSize, status);
        //        return Ok(new ResultViewModel<PagedResult<OrderResponse>>(orders));
        //    }
        //    catch
        //    {
        //        return StatusCode(500, new ResultViewModel<PagedResult<OrderResponse>>("05X02 - Falha interna no servidor"));
        //    }
        //}
    }
}

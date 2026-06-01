using Domain.Models.Enums;
using MasterData.API.Models.DTOs.Customer;
using MasterData.Domain.Models.DTOs;
using MasterData.Domain.Models.DTOs.Customer;
using MasterData.Domain.Models.DTOs.Order;
using MasterData.Domain.Models.DTOs.Product;
using MasterData.Domain.Models.Enums;
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

        [HttpGet("v1/orders/{id:int}")]
        public async Task<IActionResult> GetOrderByIdAsync([FromRoute] int id)
        {
            try
            {
                var order = await service.GetOrderByIdAsync(id);

                if (order == null)
                {
                    return NotFound(new ResultViewModel<OrderResponse>("Conteúdo não encontrado"));
                }

                return Ok(new ResultViewModel<OrderResponse>(order));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<PagedResult<OrderResponse>>("05X02 - Falha interna no servidor"));
            }
        }

        [HttpGet("v1/orders/items/{id:int}")]
        public async Task<IActionResult> GetOrderWithItensAsync([FromRoute] int id)
        {
            try
            {
                var order = await service.GetOrderItemsAsync(id);

                if (order == null)
                {
                    return NotFound(new ResultViewModel<OrderWithItensResponse>("Conteúdo não encontrado"));
                }

                return Ok(new ResultViewModel<OrderWithItensResponse>(order));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<PagedResult<OrderResponse>>("05X02 - Falha interna no servidor"));
            }
        }

        [HttpPost("v1/orders")]
        public async Task<IActionResult> PostAsync([FromBody] OrderRequest model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage)
                    .ToList();

                return BadRequest(new ResultViewModel<OrderRequest>(errors));
            }

            try
            {
                var newOrder = new OrderRequest
                {
                    IdCustomer = model.IdCustomer,
                    Total = 0,
                    Status = (int)OrderStatus.Elaboration,
                    Document = model.Document,
                    Observation = model.Observation,
                    InsertionDate = DateTime.Now
                };

                var id = await service.PostOrderAsync(newOrder);
                newOrder.Id = id;

                return Created($"v1/orders/{id}", newOrder);
            }
            catch(Exception)
            {
                return StatusCode(500, new ResultViewModel<OrderResponse>("05X02 - Falha ao inserir pedido"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<OrderResponse>("05X03 - Falha interna no servidor"));
            }
        }
        [HttpDelete("v1/orders/{id:int}")]
        public async Task<IActionResult> CancelOrderAsync([FromRoute] int id)
        {
            try
            {
                var order = await service.GetOrderByIdAsync(id);

                if (order == null)
                {
                    return NotFound(new ResultViewModel<OrderResponse>("Conteúdo não encontrado"));
                }

                await service.CancelOrder(id);

                order.Status = (int)OrderStatus.Canceled;

                return Ok(new ResultViewModel<OrderResponse>(order));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<PagedResult<OrderResponse>>("05X02 - Falha interna no servidor"));
            }
        }
    }
}

using Domain.Models.Enums;
using MasterData.Domain.Models.DTOs;
using MasterData.Domain.Models.DTOs.Order;
using MasterData.Domain.Models.ViewModels;
using MasterData.Domain.Services.API;
using Microsoft.AspNetCore.Mvc;

namespace MasterData.API.Controllers
{
    [ApiController]
    public class OrderController(OrderApiService service, ILogger<OrderController> logger) : ControllerBase
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
            catch (Exception ex)
            {
                logger.LogError(ex, $"Falha ao listar pedidos. Page={page}, PageSize={pageSize}, Status={status}");
                return StatusCode(500, new ResultViewModel<PagedResult<OrderResponse>>("Falha interna no servidor"));
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
                    logger.LogError($"Falha ao buscar pedido. OrderId={id}");
                    return NotFound(new ResultViewModel<OrderResponse>("Conteúdo não encontrado"));
                }

                return Ok(new ResultViewModel<OrderResponse>(order));
            }
            catch(Exception ex)
            {
                logger.LogError(ex, $"Falha ao buscar pedido. OrderId={id}");
                return StatusCode(500, new ResultViewModel<PagedResult<OrderResponse>>("Falha inesperada, contate o administrador do sistema"));
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
            catch (Exception ex)
            {
                logger.LogError(ex, $"Falha ao buscar pedido por Id. OrderId={id}");
                return StatusCode(500, new ResultViewModel<PagedResult<OrderResponse>>("Falha interna no servidor, contate o administrador"));
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
            catch(Exception ex)
            {
                logger.LogError(ex, "Falha ao inserir pedido: HttpPost(\"v1/orders\")");
                return StatusCode(500, new ResultViewModel<OrderResponse>("05X02 - Falha ao inserir pedido"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<OrderResponse>("05X03 - Falha interna no servidor"));
            }
        }

        [HttpPut("v1/orders/items/{id:int}")]
        public async Task<IActionResult> IncludeItemsAsync([FromRoute] int id, [FromBody] ItemRequest model)
        {
            try
            {
                var order = await service.GetOrderByIdAsync(id);
                
                if (order == null)
                {
                    return NotFound(new ResultViewModel<OrderResponse>("Conteúdo não encontrado"));
                }

                if (order.Status != (int)OrderStatus.Elaboration)
                {
                    return BadRequest(new ResultViewModel<OrderResponse>("Status do Pedido está Enviado/Cancelado."));
                }

                if (!order.IdCustomer.HasValue)
                {
                    return BadRequest(new ResultViewModel<OrderResponse>("Cliente não informado para o pedido"));
                }


                var customerId = order.IdCustomer.Value; 
                var orderItems = await service.IncludeItemsAsync(order, customerId, model);

                return Ok(new ResultViewModel<OrderWithItensResponse>(orderItems));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Falha ao incluir itens no pedido OrderId={id} ");
                return StatusCode(500, new ResultViewModel<PagedResult<OrderResponse>>("Falha interna no servidor, informe o administrador"));
            }
        }

        [HttpPut("v1/orders/{id:int}")]
        public async Task<IActionResult> SendOrderAsync([FromRoute] int id)
        {
            try
            {
                var order = await service.GetOrderByIdAsync(id);

                if (order == null)
                {
                    return NotFound(new ResultViewModel<OrderResponse>("Conteúdo não encontrado"));
                }

                if (order.Status == (int)OrderStatus.Elaboration)
                {
                    await service.SendOrderAsync(id);
                    order.Status = (int)OrderStatus.Send;
                }
                else
                {
                    return BadRequest(new ResultViewModel<OrderResponse>("Envio permitido apenas com status 'Em Elaboração'."));
                }

                return Ok(new ResultViewModel<OrderResponse>(order));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Falha ao enviar Pedido OrderId={id}");
                return StatusCode(500, new ResultViewModel<PagedResult<OrderResponse>>("05X02 - Falha interna no servidor"));
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

                if (order.Status != (int)OrderStatus.Elaboration || order.Status != (int)OrderStatus.Send)
                {
                    return BadRequest(new ResultViewModel<OrderResponse>("Ação não permitida: Pedido pode estar cancelado."));
                }

                await service.CancelOrderAsync(id);

                order.Status = (int)OrderStatus.Canceled;

                return Ok(new ResultViewModel<OrderResponse>(order));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Falha ao enviar Pedido OrderId={id}");
                return StatusCode(500, new ResultViewModel<PagedResult<OrderResponse>>("Falha interna no servidor"));
            }
        }
    }
}

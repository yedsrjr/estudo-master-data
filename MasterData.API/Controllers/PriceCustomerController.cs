using MasterData.Domain.Models.DTOs;
using MasterData.Domain.Models.DTOs.PriceCustomer;
using MasterData.Domain.Models.DTOs.Product;
using MasterData.Domain.Models.ViewModels;
using MasterData.Domain.Services.API;
using Microsoft.AspNetCore.Mvc;

namespace MasterData.API.Controllers
{
    [ApiController]
    public class PriceCustomerController(PriceCustomerApiService service) : ControllerBase
    {
        [HttpGet("v1/prices")]
        public async Task<IActionResult> GetAsync([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            try
            {
                var total = await service.CountPricesAsync();
                var prices = await service.GetPricesAsync(page, pageSize);

                var grouped = prices
                    .GroupBy(x => x.IdClient)
                    .Select(g => new PriceGroupedResponse
                    {
                        IdClient = g.Key,
                        Products = g.Select(x => new PriceItemResponse
                        {
                            IdProduct = x.IdProduct,
                            UnitValue = x.UnitValue,
                            InsertionDate = x.InsertionDate
                        }).ToList()
                    }).ToList();

                var result = new PagedResult<PriceGroupedResponse>
                {
                    Total = total,
                    Page = page,
                    PageSize = pageSize,
                    Items = grouped
                };

                return Ok(new ResultViewModel<PagedResult<PriceGroupedResponse>>(result));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<PriceCustomerResponse>("05X02 - Falha interna no servidor"));
            }
        }

        [HttpGet("v1/prices/{id:int}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            try
            {
                var price = await service.GetPriceById(id);

                if (price == null)
                {
                    return NotFound(new ResultViewModel<PriceCustomerResponse>("Conteúdo não encontrado"));
                }

                var grouped = price
                    .GroupBy(x => x.IdClient)
                    .Select(g => new PriceGroupedResponse
                    {
                        IdClient = g.Key,

                        Products = g.Select(x => new PriceItemResponse
                        {
                            IdProduct = x.IdProduct,
                            UnitValue = x.UnitValue,
                            InsertionDate = x.InsertionDate
                        }).ToList()

                    }).ToList();

                return Ok(new ResultViewModel<List<PriceGroupedResponse>>(grouped));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<PriceCustomerResponse>("05X02 - Falha interna no servidor"));
            }
        }

        [HttpPost("v1/prices/{id:int}")]
        public async Task<IActionResult> PostAsync([FromRoute] int id, [FromBody] PriceItemRequest model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage)
                    .ToList();

                return BadRequest(new ResultViewModel<PriceItemRequest>(errors));
            }

            try
            {
                var newPrice = new PriceItemRequest
                {
                    IdClient = id,
                    IdProduct = model.IdProduct,
                    UnitValue = model.UnitValue,
                    InsertionDate = DateTime.Now
                };

                var product = await service.AddAsync(newPrice);

                return Created($"v1/prices/{id}", newPrice);
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<PriceCustomerResponse>("05XE9 - Não foi possível incluir um cliente"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<PriceCustomerResponse>("05X02 - Falha interna no servidor"));
            }
        }
    }
}

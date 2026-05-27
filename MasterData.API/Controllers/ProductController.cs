using MasterData.API.Models.DTOs.Customer;
using MasterData.Domain.Models.DTOs;
using MasterData.Domain.Models.DTOs.Customer;
using MasterData.Domain.Models.DTOs.Product;
using MasterData.Domain.Models.Enums;
using MasterData.Domain.Models.ViewModels;
using MasterData.Domain.Services.API;
using Microsoft.AspNetCore.Mvc;

namespace MasterData.API.Controllers
{
    [ApiController]
    public class ProductController(ProductApiService service) : ControllerBase
    {
        [HttpGet("v1/products")]
        public async Task<ActionResult> GetAsync([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            try
            {
                var products = await service.GetProducts(page, pageSize);
                return Ok(new ResultViewModel<PagedResult<ProductResponse>>(products));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<PagedResult<ProductResponse>>("05X02 - Falha interna no servidor"));
            }
        }

        [HttpGet("v1/products/{id:int}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            try
            {
                var product = await service.GetProductById(id);

                if (product == null)
                {
                    return NotFound(new ResultViewModel<ProductResponse>("Conteúdo não encontrado"));
                }

                return Ok(new ResultViewModel<ProductResponse>(product));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<ProductResponse>("05X04 - Falha interna no servidor"));
            }
        }

        [HttpPost("v1/products")]
        public async Task<IActionResult> PostAsync([FromBody] ProductRequest model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage)
                    .ToList();

                return BadRequest(new ResultViewModel<ProductRequest>(errors));
            }

            try
            {
                var request = new ProductRequest
                {
                    Description = model.Description,
                    GrossWeight = model.GrossWeight,
                    NetWeight = model.NetWeight,
                    Quantity = model.Quantity,
                    Status = 1,
                    UpdatedAt = DateTime.Now.ToString()
                };

                var id = await service.AddAsync(request);
                request.Id = id;

                return Created($"v1/products/{id}", request);
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<List<ProductRequest>>("05XE9 - Não foi possível incluir um cliente"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<List<ProductRequest>>("05X10 - Falha interna no servidor"));
            }
        }

        [HttpPut("v1/products/{id:int}")]
        public async Task<IActionResult> PutAsync([FromRoute] int id, [FromBody] ProductRequest model)
        {
            try
            {
                var product = await service.GetProductById(id);

                if (product == null)
                {
                    return NotFound(new ResultViewModel<ProductRequest>("Conteúdo não encontrado"));
                }

                var newProduct = new ProductRequest
                {
                    Id = id,
                    Description = model.Description,
                    GrossWeight = model.GrossWeight,
                    NetWeight = model.NetWeight,
                    Quantity = model.Quantity,
                    Status = (int)Status.Active,
                    UpdatedAt = DateTime.Now.ToString()
                };

                await service.UpdateAsync(id, newProduct);

                return Ok(new ResultViewModel<ProductRequest>(newProduct));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<ProductRequest>("05XE9 - Não foi possível atualizar o cliente"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<ProductRequest>("05X10 - Falha interna no servidor"));
            }
        }

        [HttpDelete("v1/products/{id:int}")]
        public async Task<IActionResult> CancelAsync([FromRoute] int id)
        {
            try
            {
                var product = await service.GetProductById(id);

                if (product == null)
                {
                    return NotFound(new ResultViewModel<ProductResponse>("Conteúdo não encontrado"));
                }

                await service.CancelProductAsync(id);

                product.Status = (int)Status.Inactive;

                return Ok(new ResultViewModel<ProductResponse>(product));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<ProductResponse>("05X04 - Falha interna no servidor"));
            }
        }
    }
}

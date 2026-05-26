using MasterData.API.Models.DTOs.Customer;
using MasterData.Domain.Models.DTOs;
using MasterData.Domain.Models.DTOs.Customer;
using MasterData.Domain.Models.ViewModels;
using MasterData.Domain.Services.API;
using Microsoft.AspNetCore.Mvc;

namespace MasterData.API.Controllers
{
    [ApiController]
    public class CustomerController(CustomerApiService service) : ControllerBase
    {
        [HttpGet("v1/customers")]
        public async Task<ActionResult> GetAsync([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            try
            {
                var customers = await service.GetCustomersAsync(page, pageSize);
                return Ok(new ResultViewModel<PagedResult<CustomerResponse>>(customers));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<PagedResult<CustomerResponse>>("05X02 - Falha interna no servidor"));
            }
        }

        [HttpGet("v1/customers/{id:int}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            try
            {
                var customer = await service.GetByIdAsync(id);

                if (customer == null)
                {
                    return NotFound(new ResultViewModel<CustomerResponse>("Conteúdo não encontrado"));
                }

                return Ok(new ResultViewModel<CustomerResponse>(customer));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<CustomerResponse>("05X04 - Falha interna no servidor"));
            }

        }

        [HttpPost("v1/customers")]
        public async Task<IActionResult> PostAsync([FromBody] CustomerRequest model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage)
                    .ToList();

                return BadRequest(new ResultViewModel<CustomerRequest>(errors));
            }

            try
            {
                var request = new CustomerRequest
                {
                    Name = model.Name,
                    ShortName = model.ShortName,
                    CpfCnpj = model.CpfCnpj,
                    Status = 1,
                    UpdatedAt = DateTime.Now.ToString()
                };

                var id = await service.AddAsync(request);
                request.Id = id;

                return Created($"v1/customers/{id}", request);
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<List<CustomerRequest>>("05XE9 - Não foi possível incluir um cliente"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<List<CustomerRequest>>("05X10 - Falha interna no servidor"));
            }
        }
    }
}

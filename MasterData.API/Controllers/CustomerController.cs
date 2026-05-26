using Domain.Services;
using MasterData.Domain.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace MasterData.API.Controllers
{
    [ApiController]
    public class CustomerController(ClienteService service) : ControllerBase
    {
        //[HttpGet("v1/customer")]
        //public ActionResult GetAsync()
        //{
        //    try
        //    {
                
        //    }
        //    catch
        //    {
        //        return StatusCode(500, new ResultViewModel<List<Category>>("05X02 - Falha interna no servidor"));
        //    }
        //}

        
    }
}

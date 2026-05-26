using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace MasterData.Domain.Controllers;

[ApiController]
public class HomeController(IConfiguration config) : ControllerBase
{
    [HttpGet("")]
    public IActionResult Get()
    {
        var env = config.GetValue<string>("Env");
        return Ok(new
        {
            environment = env
        });
    }

}

using MasterData.Domain.Extensions;
using MasterData.Domain.Models.DTOs.User;
using MasterData.Domain.Services.API;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace MasterData.Domain.Controllers;

[ApiController]
public class HomeController(IConfiguration config, TokenService serviceToken) : ControllerBase
{
    [HttpGet("")]
    public IActionResult Get()
    {
        var user = new User(1, "edson", "eds@gmail.com", "xyz", new[] { "student", "premium" });
        var token = serviceToken.Create(user);
        var env = config.GetValue<string>("Env");
        return Ok(new
        {
            environment = env,
            token = token
        });
    }

    [Authorize]
    [HttpGet("/restrito")]
    public IActionResult Restrito()
    {
        var claims = new
        {
            id = User.Id(),
            name = User.Name(),
            email = User.Email(),
            givenName = User.GivenName() 
        };
        return Ok(claims);
    }

    [Authorize("admin")]
    [HttpGet("/admin")]
    public IActionResult Restrito2()
    {
        return Ok(new
        {
            message = "Você tem acesso!"
        });
    }

}

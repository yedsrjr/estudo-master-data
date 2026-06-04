using MasterData.Domain.Models.DTOs.User;
using MasterData.Domain.Services.API;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Security.Claims;


namespace MasterData.Domain.Controllers;

[ApiController]
public class HomeController(IConfiguration config, TokenService tokenService, ILogger<HomeController> logger) : ControllerBase
{
    [HttpGet("/login")]
    public IActionResult Get()
    {
        try
        {
            var env = config.GetValue<string>("Env");
            var user = new User(1, "edson", "edson@danone.com", "xyz", ["student", "basic", "admin"], "IT");
            var tk = tokenService.Create(user); 

            logger.LogInformation(
                "Login realizado para UserId={UserId}, Name={Name}, Email={Email}, Environment={Environment}",
                user.Id,
                user.Name,
                user.Email,
                env);

            return Ok(new
            {
                environment = env,
                token = tk
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Falha ao realizar login");
            return StatusCode(500, "Erro interno");
        }
    }

    [HttpGet("/policy1")]
    [Authorize(Policy = "OnlyUser1")]
    public IActionResult Get1()
    {
        return Ok("Only User 1: Access Successful");
    }

    [HttpGet("/log-error-test")]
    public IActionResult LogErrorTest()
    {
        try
        {
            throw new InvalidOperationException("Teste de erro manual no HomeController");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, " Catch: Log de erro manual disparado em /log-error-test");
            return StatusCode(500, " Catch: Erro interno de teste");
        }
    }

    [HttpGet("/policy2")]
    [Authorize(Policy = "OnlyEdsonEmail")]
    public IActionResult Get2()
    {
        return Ok("Only Edson Email : Access Successful");
    }

    [HttpGet("/policy3")]
    [Authorize(Policy = "Developers")]
    public IActionResult Get3()
    {
        return Ok("Only Developers: Access Successful");
    }

    [HttpGet("/policy4")]
    [Authorize(Policy = "MasterAdmin")]
    public IActionResult Get4()
    {
        return Ok("Only SuperAdmin: Access Successful");
    }

    [HttpGet("/policy5")]
    [Authorize(Policy = "InternalUsers")]
    public IActionResult Get5()
    {
        return Ok("Only Internal: Access Successful");
    }

    [HttpGet("/policy6")]
    [Authorize(Policy = "OnlyIT")]
    public IActionResult Get6()
    {
        try
        {
            var name = User.FindFirst(ClaimTypes.GivenName)?.Value;
            var email = User.FindFirst(ClaimTypes.Name)?.Value;

            logger.LogInformation(
                "Acesso autorizado em /policy6 para Name={Name}, Email={Email}",
                name,
                email);

            return Ok("Only Internal: Access Successful");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, " Catchh: Erro inesperado");
            return StatusCode(500, "Erro interno");
        }
        
    }
}

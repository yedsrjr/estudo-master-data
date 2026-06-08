using MasterData.Domain.Extensions;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services));

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddApiInfrastructure(builder.Configuration);

builder.Services.AddRepositoriesApi(); 
builder.Services.AddServicesApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.MapScalarApiReference("v1/documentation", options =>
    {
        options
            .WithTitle("Pedidos API")
            .WithTheme(ScalarTheme.DeepSpace)
            .WithDefaultHttpClient(
                ScalarTarget.CSharp,
                ScalarClient.HttpClient);
    });
}

app.UseHttpsRedirection();        
//app.UseSerilogRequestLogging();   
app.UseAuthentication();          
app.UseAuthorization();

app.MapControllers();

app.Logger.LogInformation("API iniciada com Serilog em {DateTime}", DateTime.Now);

app.Run();

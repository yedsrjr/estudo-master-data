using Domain.Repository;
using Domain.Services;
using JJMasterData.Web.Configuration;
using MasterData.Domain.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMemoryCache();
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<ClienteService>();
builder.Services.AddTransient<ProdutoService>();
builder.Services.AddTransient<PedidoService>();
builder.Services.AddScoped<OrderValidate>();
builder.Services.AddTransient<PedidoItemService>();
builder.Services.AddTransient<PriceCustomerService>();
builder.Services.AddTransient<LogOrderService>();
builder.Services.AddTransient<DashboardService>();
builder.Services.AddScoped<DashboardRepository>();
builder.Services.AddScoped<PedidoRepository>();
builder.Services.AddJJMasterDataWeb();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession(); //Session is very important
app.MapDataDictionary();
app.MapMasterData();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=MasterDataDash}/{action=Index}/{id?}")
    .WithStaticAssets();


await app.UseMasterDataSeedingAsync();

app.Run();

using JJMasterData.Web.Configuration;
using MasterData.Domain.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMemoryCache();
builder.Services.AddControllersWithViews();

builder.Services.AddServices();
builder.Services.AddRepositories();

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

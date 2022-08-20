
using ShopOnline.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components.WebAssembly.Server;
using ShopOnline.Api.Repositories;
using Microsoft.Net.Http.Headers;
using ShopOnline.Api.Repositories.Contracts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContextPool<ShopOnlineDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ShopOnlineConnection")));
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();

// Configure the HTTP request pipeline.
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(policy =>
    policy.WithOrigins("http://localhost:7080", "https://localhost:7080")
    .AllowAnyMethod()
    .WithHeaders(HeaderNames.ContentType)
    );
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();


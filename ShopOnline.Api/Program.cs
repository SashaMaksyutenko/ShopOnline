
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Net.Http.Headers;
//using ShopOnline.Api.Data;
//using ShopOnline.Api.Repositories;
//using ShopOnline.Api.Repositories.Contracts;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.
//builder.Services.AddControllersWithViews();
//builder.Services.AddControllers();
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
//builder.Services.AddDbContextPool<ShopOnlineDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ShopOnlineConnection")));
//builder.Services.AddScoped<IProductRepository, ProductRepository>();
//builder.Services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();

//// Configure the HTTP request pipeline.
//var app = builder.Build();

//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
//app.UseCors(policy =>
//    policy.WithOrigins("http://localhost:7080", "https://localhost:7080")
//    .AllowAnyMethod()
//    .WithHeaders(HeaderNames.ContentType)
//    );
//app.UseHttpsRedirection();
//app.UseStaticFiles();

//app.UseRouting();

//app.UseAuthorization();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

//app.Run();

using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using ShopOnline.Api.Data;
using ShopOnline.Api.Repositories;
using ShopOnline.Api.Repositories.Contracts;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextPool<ShopOnlineDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ShopOnlineConnection")));
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Open", builder => builder.AllowAnyOrigin().AllowAnyHeader());
});

builder.Services.AddControllers();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    using var context = scope.ServiceProvider.GetService<ShopOnlineDbContext>();
    context!.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseAuthorization();
app.UseCors("Open");


app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseRouting();
app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
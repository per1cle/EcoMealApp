using EcoMeal.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using EcoMeal.DataAccess.Entities;
using EcoMeal.DataAccess.Repositories;
using EcoMeal.BusinessLogic.Services;
using EcoMeal.BusinessLogic.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddDbContext<EcoMealDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("EcoMealDb")));

builder.Services.AddScoped<IRepository<Business>, BaseRepository<Business>>();
builder.Services.AddScoped<IRepository<BusinessType>, BaseRepository<BusinessType>>();
builder.Services.AddScoped<IRepository<Package>, BaseRepository<Package>>();
builder.Services.AddScoped<IRepository<PackageType>, BaseRepository<PackageType>>();

builder.Services.AddScoped<IBusinessService, BusinessService>();
builder.Services.AddScoped<IBusinessTypeService, BusinessTypeService>();
builder.Services.AddScoped<IPackageService, PackageService>();
builder.Services.AddScoped<IPackageTypeService, PackageTypeService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin",
        corsPolicyBuilder =>
        {
            corsPolicyBuilder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

var app = builder.Build();

app.UseHttpsRedirection();

app.UseCors("AllowAnyOrigin");

app.UseAuthorization();
app.MapControllers();

app.Run();
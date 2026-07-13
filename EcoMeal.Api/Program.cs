using EcoMeal.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using EcoMeal.DataAccess.Entities;
using EcoMeal.DataAccess.Repositories;
using EcoMeal.BusinessLogic.Services;
using EcoMeal.BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddDbContext<EcoMealDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("EcoMealDb")));

builder.Services.AddIdentity<User, Role>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<EcoMealDbContext>()
.AddDefaultTokenProviders();

var secretKey = builder.Configuration["JwtSettings:SecretKey"] ?? "OneOfTheBestSecretKeysEverUsedInTheHistoryOfTheWorld123!";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});

builder.Services.AddScoped<IRepository<Business>, BaseRepository<Business>>();
builder.Services.AddScoped<IRepository<BusinessType>, BaseRepository<BusinessType>>();
builder.Services.AddScoped<IRepository<Package>, BaseRepository<Package>>();
builder.Services.AddScoped<IRepository<PackageType>, BaseRepository<PackageType>>();
builder.Services.AddScoped<IRepository<Order>, BaseRepository<Order>>();
builder.Services.AddScoped<IRepository<OrderPackage>, BaseRepository<OrderPackage>>();

builder.Services.AddScoped<IBusinessService, BusinessService>();
builder.Services.AddScoped<IBusinessTypeService, BusinessTypeService>();
builder.Services.AddScoped<IPackageService, PackageService>();
builder.Services.AddScoped<IPackageTypeService, PackageTypeService>();
builder.Services.AddScoped<IOrderService, OrderService>();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
using Microsoft.EntityFrameworkCore;
using WebBanAoo.Data;
using WebBanAoo.Mapper;
using WebBanAoo.Mapper.impl;
using WebBanAoo.Models;
using WebBanAoo.Models.Status;

using WebBanAoo.Service;
using WebBanAoo.Service.impl;
using System.Text.Json.Serialization;
using WebBanAoo.Ultility;
using Service.impl;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionStr = builder.Configuration.GetConnectionString("MySQL");

builder.Services.AddDbContext<ApplicationDbContext>(o =>
    o.UseMySql(connectionStr, new MySqlServerVersion(new Version(8, 0, 33))));


builder.Services.AddScoped<IBrandMapper, BrandMapper>();
builder.Services.AddScoped<IBrandService, BrandService>();
builder.Services.AddScoped<ICategoryMapper, CategoryMapper>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IColorMapper, ColorMapper>();
builder.Services.AddScoped<IColorService, ColorService>();
builder.Services.AddScoped<ICustomerMapper, CustomerMapper>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IEmployeeMapper, EmployeeMapper>();
builder.Services.AddScoped<IEmployeeService , EmployeeService>();
builder.Services.AddScoped<IOrderDetailMapper, OrderDetailMapper>();
builder.Services.AddScoped<IOrderDetailService , OrderDetailService>();
builder.Services.AddScoped<IOrderMapper, OrderMapper>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IProductDetailMapper, ProductDetailMapper>();
builder.Services.AddScoped<IProductDetailService , ProductDetailService>();
builder.Services.AddScoped<IProductMapper, ProductMapper>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IRoleMapper, RoleMapper>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<ISaleMapper, SaleMapper>();
builder.Services.AddScoped<ISaleService, SaleService>();
builder.Services.AddScoped<ISizeMapper, SizeMapper>();
builder.Services.AddScoped<ISizeService, SizeService>();
builder.Services.AddScoped<IVoucherMapper, VoucherMapper>();
builder.Services.AddScoped<IVoucherService, VoucherService>();
builder.Services.AddScoped<IProductImageMapper, ProductImageMapper>();
builder.Services.AddScoped<IProductImageService, ProductImageService>();
//Chuy?n ??i enum 
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddScoped(typeof(Validation<>));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

builder.Services.AddAuthorization();

builder.Services.AddScoped<ICustomPasswordHasher, CustomPasswordHasher>();
builder.Services.AddScoped<IAuthService, AuthService>();


//  CORS service
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder
                .AllowAnyOrigin()     //  origin
                .AllowAnyMethod()      //  HTTP methods
                .AllowAnyHeader();     //  headers
        });
});


var app = builder.Build();

app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

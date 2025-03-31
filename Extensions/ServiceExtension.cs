using WebBanAoo.Mapper.impl;
using WebBanAoo.Mapper;
using WebBanAoo.Models.Mapper;
using WebBanAoo.Service;
using WebBanAoo.Service.impl;
using WebBanAoo.Ultility;

namespace WebBanAoo.Extensions;

public static class ServiceExtension
{
    public static IServiceCollection ServiceExtend(this IServiceCollection services)
    {
        services.AddScoped<IBrandMapper, BrandMapper>();
        services.AddScoped<IBrandService, BrandService>();
        services.AddScoped<ICategoryMapper, CategoryMapper>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IColorMapper, ColorMapper>();
        services.AddScoped<IColorService, ColorService>();
        services.AddScoped<ICustomerMapper, CustomerMapper>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IEmployeeMapper, EmployeeMapper>();
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IOrderDetailMapper, OrderDetailMapper>();
        services.AddScoped<IOrderDetailService, OrderDetailService>();
        services.AddScoped<IOrderMapper, OrderMapper>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IProductDetailMapper, ProductDetailMapper>();
        services.AddScoped<IProductDetailService, ProductDetailService>();
        services.AddScoped<IProductMapper, ProductMapper>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IRoleMapper, RoleMapper>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<ISaleMapper, SaleMapper>();
        services.AddScoped<ISaleService, SaleService>();
        services.AddScoped<ISizeMapper, SizeMapper>();
        services.AddScoped<ISizeService, SizeService>();
        services.AddScoped<IVoucherMapper, VoucherMapper>();
        services.AddScoped<IVoucherService, VoucherService>();
        services.AddScoped<IProductImageMapper, ProductImageMapper>();
        services.AddScoped<IProductImageService, ProductImageService>();
        services.AddScoped<ICartMapper, CartMapper>();
        services.AddScoped<ICartService, CartService>();
        services.AddScoped<ICustomerVoucherMapper, CustomerVoucherMapper>();
        services.AddScoped<ICustomerVoucherService, CustomerVoucherService>();
        services.AddScoped(typeof(Validation<>));
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IBestSellerService, BestSellerService>();

        return services;
    }
}

using Microsoft.EntityFrameworkCore;
using WebBanAoo.Data;
using WebBanAoo.Models.DTO.Response;

namespace WebBanAoo.Service.impl;

public class BestSellerService : IBestSellerService
{
    private readonly ApplicationDbContext _context;

    public BestSellerService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<BestSellerResponse>> GetBestSellersAsync()
    {
        var orderDetails = await _context.OrderDetails
        .GroupBy(od => od.ProductDetailId)
        .Select(g => new
        {
            ProductDetailId = g.Key,
            TotalQuantity = g.Sum(od => od.Quantity)
        })
        .OrderByDescending(g => g.TotalQuantity)
        .ToListAsync();

        if (orderDetails == null || !orderDetails.Any())
            throw new Exception("Không có sản phẩm bán chạy nào.");

        var response = orderDetails.Select(o => new BestSellerResponse
        {
            ProductDetailId = o.ProductDetailId,
            TotalQuantity = o.TotalQuantity
        });

        return response;
    }
}

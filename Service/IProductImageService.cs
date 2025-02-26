using WebBanAoo.Models;
using WebBanAoo.Models.DTO.Request.ProductImage;
using WebBanAoo.Models.DTO.Response;

namespace WebBanAoo.Service;

public interface IProductImageService
{
    public Task<IEnumerable<ProductImageResponse>> GetAllProductImageAsync();
    
    public Task<ProductImageResponse> UpdateProductImageAsync(int id, ProductImageUpdate update);
    public Task<ProductImageResponse> CreateProductImageAsync(ProductImageCreate create);
    public Task<bool> HardDeleteProductImageAsync(int id);

    public Task<IEnumerable<ProductImageResponse>> FindProductImageByProductDetailIdAsync(int id);
    public Task<string> CheckUniqueCodeAsync();
}

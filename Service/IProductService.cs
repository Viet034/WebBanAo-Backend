using WebBanAoo.Models.DTO.Request.Product;
using WebBanAoo.Models.DTO.Response;
using static WebBanAoo.Models.Status.Status;

namespace WebBanAoo.Service
{
    public interface IProductService
    {
        public Task<IEnumerable<ProductResponse>> GetAllProductsAsync();
        public Task<IEnumerable<ProductResponse>> SearchByKeyAsync(string key);
        public Task<ProductResponse> UpdateProductAsync(int id, ProductUpdate update);
        public Task<ProductResponse> CreateProductAsync(ProductCreate create);
        public Task<bool> HardDeleteProductAsync(int id);
        public Task<ProductResponse> SoftDeleteProductAsync(int id, ProductStatus newStatus);
        public Task<ProductResponse> FindProductByIdAsync(int id);
        public Task<string> CheckUniqueCodeAsync();
    }
}

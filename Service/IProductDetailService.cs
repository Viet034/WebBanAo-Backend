using WebBanAoo.Models.DTO.Response;
using static WebBanAoo.Models.Status.Status;
using WebBanAoo.Models.DTO.Request.ProductDetail;

namespace WebBanAoo.Service
{
    public interface IProductDetailService
    {
        public Task<IEnumerable<ProductDetailResponse>> GetAllProductDetailAsync();
        public Task<IEnumerable<ProductDetailResponse>> SearchProductDetailByKeyAsync(string key);
        public Task<ProductDetailResponse> UpdateProductDetailAsync(int id, ProductDetailUpdate update);
        public Task<ProductDetailResponse> CreateProductDetailAsync(ProductDetailCreate create);
        public Task<bool> HardDeleteProductDetailAsync(int id);
        public Task<ProductDetailResponse> SoftDeleteProductDetailAsync(int id, ProductDetailStatus newStatus);
        public Task<ProductDetailResponse> FindProductDetailByIdAsync(int id);
        public Task<IEnumerable<ProductDetailResponse>> FindProductDetailByProductIdAsync(int id);
        public Task<string> CheckUniqueCodeAsync();
    }
}

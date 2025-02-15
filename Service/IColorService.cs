using WebBanAoo.Models.DTO.Response;
using static WebBanAoo.Models.Status.Status;
using WebBanAoo.Models.DTO.Request.Color;

namespace WebBanAoo.Service
{
    public interface IColorService
    {
        public Task<IEnumerable<ColorResponse>> GetAllColorProductsAsync();
        public Task<IEnumerable<ColorResponse>> SearchColorByKeyAsync(string key);
        public Task<ColorResponse> UpdateProductColorAsync(int id, ColorUpdate update);
        public Task<ColorResponse> CreateProductColorAsync(ColorCreate create);
        public Task<bool> HardDeleteProductColorAsync(int id);
        public Task<ColorResponse> SoftDeleteProductColorAsync(int id, ColorStatus newStatus);
        public Task<ColorResponse> FindProductColorByIdAsync(int id);
        public Task<string> CheckUniqueCodeAsync();
    }
}

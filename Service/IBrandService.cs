using WebBanAoo.Models.DTO.Response;
using static WebBanAoo.Models.Status.Status;
using WebBanAoo.Models.DTO.Request.Brand;

namespace WebBanAoo.Service;

public interface IBrandService
{
    public Task<IEnumerable<BrandResponse>> GetAllBrandsAsync();
    public Task<IEnumerable<BrandResponse>> SearchBrandByKeyAsync(string key);
    public Task<BrandResponse> UpdateBrandAsync(int id, BrandUpdate update);
    public Task<BrandResponse> CreateBrandAsync(BrandCreate create);
    public Task<bool> HardDeleteBrandAsync(int id);
    
    public Task<BrandResponse> FindBrandByIdAsync(int id);
    public Task<string> CheckUniqueCodeAsync();
}

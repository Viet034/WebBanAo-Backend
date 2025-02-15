using WebBanAoo.Models.DTO.Response;
using static WebBanAoo.Models.Status.Status;
using WebBanAoo.Models.DTO.Request.Size;

namespace WebBanAoo.Service
{
    public interface ISizeService
    {
        public Task<IEnumerable<SizeResponse>> GetAllSizeAsync();
        public Task<IEnumerable<SizeResponse>> SearchSizeByKeyAsync(string key);
        public Task<SizeResponse> UpdateSizeAsync(int id, SizeUpdate update);
        public Task<SizeResponse> CreateSizeAsync(SizeCreate create);
        public Task<bool> HardDeleteSizeAsync(int id);
        public Task<SizeResponse> SoftDeleteSizeAsync(int id, SizeStatus newStatus);
        public Task<SizeResponse> FindSizeByIdAsync(int id);
        public Task<string> CheckUniqueCodeAsync();
    }
}

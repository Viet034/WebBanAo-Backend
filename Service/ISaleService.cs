using WebBanAoo.Models.DTO.Response;
using static WebBanAoo.Models.Status.Status;
using WebBanAoo.Models.DTO.Request.Sale;

namespace WebBanAoo.Service
{
    public interface ISaleService
    {
        public Task<IEnumerable<SaleResponse>> GetAllSaleAsync();
        public Task<IEnumerable<SaleResponse>> SearchSaleByKeyAsync(string key);
        public Task<SaleResponse> UpdateSaleAsync(int id, SaleUpdate update);
        public Task<SaleResponse> CreateSaleAsync(SaleCreate create);
        public Task<bool> HardDeleteSaleAsync(int id);
        public Task<SaleResponse> SoftDeleteSaleAsync(int id, SaleStatus newStatus);
        public Task<SaleResponse> FindSaleByIdAsync(int id);
        public Task<string> CheckUniqueCodeAsync();
    }
}

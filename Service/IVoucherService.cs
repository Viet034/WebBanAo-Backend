using WebBanAoo.Models.DTO.Response;
using static WebBanAoo.Models.Status.Status;
using WebBanAoo.Models.DTO.Request.Voucher;

namespace WebBanAoo.Service
{
    public interface IVoucherService
    {
        public Task<IEnumerable<VoucherResponse>> GetAllVoucherAsync();
        public Task<IEnumerable<VoucherResponse>> SearchVoucherByKeyAsync(string key);
        public Task<VoucherResponse> UpdateVoucherAsync(int id, VoucherUpdate update);
        public Task<VoucherResponse> CreateVoucherAsync(VoucherCreate create);
        public Task<bool> HardDeleteVoucherAsync(int id);
        public Task<VoucherResponse> SoftDeleteVoucherAsync(int id, VoucherStatus newStatus);
        public Task<VoucherResponse> FindVoucherByIdAsync(int id);
        public Task<string> CheckUniqueCodeAsync();
    }
}

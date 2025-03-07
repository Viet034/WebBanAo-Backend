using WebBanAoo.Models.DTO.Request.CustomerVoucher;
using WebBanAoo.Models.DTO.Response;
using static WebBanAoo.Models.Status.Status;

namespace WebBanAoo.Service
{
    public interface ICustomerVoucherService
    {
        Task<CustomerVoucherResponse> AssignVoucherToCustomerAsync(CustomerVoucherCreate create);
        Task<IEnumerable<CustomerVoucherResponse>> GetVouchersByCustomerIdAsync(int customerId);
        Task<CustomerVoucherResponse> UpdateCustomerVoucherStatusAsync(int customerVoucherId, CustomerVoucherStatus newStatus);
        Task<bool> DeleteCustomerVoucherAsync(int customerVoucherId);
        Task<bool> CheckVoucherAvailabilityAsync(int voucherId);
    }
} 
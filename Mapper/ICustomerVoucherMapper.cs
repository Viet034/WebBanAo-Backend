using WebBanAoo.Models;
using WebBanAoo.Models.DTO.Request.CustomerVoucher;
using WebBanAoo.Models.DTO.Response;

namespace WebBanAoo.Mapper
{
    public interface ICustomerVoucherMapper
    {
        Customer_Voucher CreateToEntity(CustomerVoucherCreate create);
        CustomerVoucherResponse EntityToResponse(Customer_Voucher entity);
        IEnumerable<CustomerVoucherResponse> ListEntityToResponse(IEnumerable<Customer_Voucher> entities);
    }
} 
using static WebBanAoo.Models.Status.Status;

namespace WebBanAoo.Models.DTO.Request.CustomerVoucher
{
    public class CustomerVoucherCreate
    {
        public int CustomerId { get; set; }
        public int VoucherId { get; set; }
        public CustomerVoucherStatus Status { get; set; } = CustomerVoucherStatus.Active;

        public CustomerVoucherCreate()
        {
        }

        public CustomerVoucherCreate(int customerId, int voucherId, CustomerVoucherStatus status)
        {
            CustomerId = customerId;
            VoucherId = voucherId;
            Status = status;
        }
    }
} 
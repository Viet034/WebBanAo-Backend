using static WebBanAoo.Models.Status.Status;

namespace WebBanAoo.Models
{
    public class Customer_Voucher
    {
        public int Id { get; set; }
        public CustomerVoucherStatus Status { get; set; } = CustomerVoucherStatus.Active;
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        public int VoucherId { get; set; }
        public virtual Voucher Voucher { get; set; }
    }
}

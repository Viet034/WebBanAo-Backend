using WebBanAoo.Models.ultility;
using static WebBanAoo.Models.Status.Status;

namespace WebBanAoo.Models;

public class Voucher : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public VoucherStatus Status { get; set; } = VoucherStatus.Active;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int Quantity { get; set; }
    public decimal DiscountValue { get; set; } //Giá trị giảm giá
    public decimal MinimumOrderValue { get; set; } //Giá trị đơn tối thiểu
    public decimal MaxDiscount { get; set; } //Giảm tối đa
    public virtual ICollection<Customer_Voucher> Customer_Vouchers { get; set; } = new List<Customer_Voucher>();
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}

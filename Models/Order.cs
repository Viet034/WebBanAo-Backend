using WebBanAoo.Ultility;
using static WebBanAoo.Models.Status.Status;

namespace WebBanAoo.Models;

public class Order : BaseEntity
{

    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public decimal InitialTotalAmount { get; set; } // tổng tiền ban đầu
    public decimal TotalAmount { get; set; }
    public string? Note { get; set; }
    public DateTime OrderDate { get; set; }
    public int CustomerId { get; set; }
    public virtual Customer Customer { get; set; }
    public int EmployeeId { get; set; }
    public virtual Employee Employee { get; set; }
    public int VoucherId { get; set; }
    public virtual Voucher Voucher { get; set; }
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}

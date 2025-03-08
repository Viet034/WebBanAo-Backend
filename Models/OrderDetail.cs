using WebBanAoo.Ultility;
using static WebBanAoo.Models.Status.Status;

namespace WebBanAoo.Models;

public class OrderDetail : BaseEntity
{
    public OrderDetailStatus Status { get; set; } = OrderDetailStatus.Processed;
    public int Quantity { get; set; } 
    public decimal UnitPrice { get; set; } // Giá mỗi đơn vị sản phẩm
    public decimal? Discount { get; set; } // Chiết khấu (nếu có)
    public decimal TotalAmount { get; set; } // Tổng tiền cho sản phẩm (sau chiết khấu)
    public string? Note { get; set; } 
    public int OrderId { get; set; } 
    public virtual Order Order { get; set; } 
    public int ProductDetailId { get; set; }
    public virtual ProductDetail ProductDetail { get; set; }

}


using System.ComponentModel.DataAnnotations;
using static WebBanAoo.Models.Status.Status;

namespace WebBanAoo.Models.DTO;

public class OrderDetailDTO
{
    public int Id { get; set; }
    [Required]
    public string Code { get; set; }
    [EnumDataType(typeof(OrderDetailStatus))]
    public OrderDetailStatus Status { get; set; } = OrderDetailStatus.Processed;
    [Required]
    public int Quantity { get; set; }
    [Required]
    public decimal UnitPrice { get; set; } // Giá mỗi đơn vị sản phẩm
    [Required]
    public decimal Discount { get; set; } // Chiết khấu (nếu có)
    [Required]
    public decimal TotalAmount { get; set; } // Tổng tiền cho sản phẩm (sau chiết khấu)
    public string? Note { get; set; }
    [Required]
    public int OrderId { get; set; }
    [Required]
    public int ProductDetailId { get; set; }

}

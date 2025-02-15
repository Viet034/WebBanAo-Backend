using System.ComponentModel.DataAnnotations;
using static WebBanAoo.Models.Status.Status;

namespace WebBanAoo.Models.DTO;

public class OrderDTO
{
    public int Id { get; set; }
    [Required]
    public string Code { get; set; }
    [EnumDataType(typeof(OrderStatus))]
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    [Required]
    public decimal InitialTotalAmount { get; set; } // tổng tiền ban đầu
    [Required]
    public decimal TotalAmount { get; set; }
    public string? Note { get; set; }
    [Required]
    public DateTime OrderDate { get; set; }
    [Required]
    public DateTime CreateDate { get; set; } = DateTime.UtcNow;
}

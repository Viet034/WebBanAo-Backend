using System.ComponentModel.DataAnnotations;
using static WebBanAoo.Models.Status.Status;

 namespace WebBanAoo.Models.DTO;

public class VoucherDTO
{
    public int Id { get; set; }
    [Required]
    public string Code { get; set; }
    [Required(ErrorMessage = "Voucher Name is Missing")]
    public string Name { get; set; }
    public string Description { get; set; }
    [EnumDataType(typeof(VoucherStatus))]
    public VoucherStatus Status { get; set; } = VoucherStatus.Active;
    [Required]
    public DateTime StartDate { get; set; }
    [Required]
    public DateTime EndDate { get; set; }
    [Required]
    public int Quantity { get; set; }
    [Required]
    public decimal DiscountValue { get; set; } //Giá trị giảm giá
    [Required]
    public decimal MinimumOrderValue { get; set; } //Giá trị đơn tối thiểu
    [Required]
    public decimal MaxDiscount { get; set; } //Giảm tối đa
}

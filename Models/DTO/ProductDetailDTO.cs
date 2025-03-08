using System.ComponentModel.DataAnnotations;
using static WebBanAoo.Models.Status.Status;

namespace WebBanAoo.Models.DTO;

public class ProductDetailDTO
{
    public int Id { get; set; }
    [Required]
    public string Code { get; set; }
    [Required(ErrorMessage = "Product Name is Missing")]
    public string Name { get; set; }
    [Required]
    public decimal Price { get; set; }
    [EnumDataType(typeof(ProductDetailStatus))]
    public ProductDetailStatus Status { get; set; } = ProductDetailStatus.Available;
    public int Quantity { get; set; }
    [Required]
    public int ProductId { get; set; }
    [Required]
    public int ColorId { get; set; }
    [Required]
    public int SizeId { get; set; }
}

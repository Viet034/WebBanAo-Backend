using System.ComponentModel.DataAnnotations;
using static WebBanAoo.Models.Status.Status;

namespace WebBanAoo.Models.DTO;

public class ProductDTO
{
    public int Id { get; set; }
    [Required]
    public string Code { get; set; }
    [Required(ErrorMessage = "Product Name is Missing")]
    public string ProductName { get; set; }
    public string Description { get; set; }
    

    [EnumDataType(typeof(ProductStatus))]
    public ProductStatus Status { get; set; } = ProductStatus.Available;
    [Required]
    public int CategoryId { get; set; }
    [Required]
    public int BrandId { get; set; }
}

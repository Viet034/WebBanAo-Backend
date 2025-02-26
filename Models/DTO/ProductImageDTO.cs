using static WebBanAoo.Models.Status.Status;

namespace WebBanAoo.Models.DTO;

public class ProductImageDTO
{
    public int Id { get; set; }
    public string Code { get; set; }
    public int ProductDetailId { get; set; }
    
    public ProductImageStatus Status { get; set; } = ProductImageStatus.Active;
    public string Url { get; set; }
}

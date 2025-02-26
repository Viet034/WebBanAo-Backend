using static WebBanAoo.Models.Status.Status;

namespace WebBanAoo.Models.DTO.Request.ProductImage;

public class ProductImageCreate
{
    
    public string Code { get; set; }
    public int ProductDetailId { get; set; }
    
    public ProductImageStatus Status { get; set; } = ProductImageStatus.Active;
    public string Url { get; set; }

    public ProductImageCreate(string code, int productDetailId, ProductImageStatus status, string url)
    {
        Code = code;
        ProductDetailId = productDetailId;
        Status = status;
        Url = url;
    }
}

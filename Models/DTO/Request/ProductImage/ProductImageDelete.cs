using static WebBanAoo.Models.Status.Status;

namespace WebBanAoo.Models.DTO.Request.ProductImage;

public class ProductImageDelete
{
    public int Id { get; set; }
    public string Code { get; set; }
    public int ProductDetailId { get; set; }
    
    public ProductImageStatus Status { get; set; } = ProductImageStatus.Active;
    public string Url { get; set; }
    public ProductImageDelete(int id, string code, int productDetailId, ProductImageStatus status, string url)
    {
        Id = id;
        Code = code;
        ProductDetailId = productDetailId;
        Status = status;
        Url = url;
    }
}

using static WebBanAoo.Models.Status.Status;

namespace WebBanAoo.Models
{
    public class ProductImage
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int ProductDetailId { get; set; }
        public virtual ProductDetail ProductDetail { get; set; }
        public ProductImageStatus Status { get; set; } = ProductImageStatus.Active;
        public string Url { get; set; }
    }
}

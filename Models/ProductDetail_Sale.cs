using static WebBanAoo.Models.Status.Status;

namespace WebBanAoo.Models;

public class ProductDetail_Sale
{
    public int Id { get; set; }
    public int ProductDetailId { get; set; }
    public virtual ProductDetail ProductDetail { get; set; }
    public int SaleId { get; set; }
    public virtual Sale Sale { get; set; }  
    public ProductDetailSaleStatus Status { get; set; } = ProductDetailSaleStatus.Scheduled;
}

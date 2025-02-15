using static WebBanAoo.Models.Status.Status;

namespace WebBanAoo.Models;

public class Cart_ProductDetail
{
    public int Id { get; set;}
    public int Quantity { get; set;}
    public CartProductDetailStatus Status { get; set;} = CartProductDetailStatus.Available;
    public int ProductDetailId { get; set;}
    public virtual ProductDetail ProductDetail { get; set;}
    public int CartId { get; set; }
    public virtual Cart Cart { get; set; }
}

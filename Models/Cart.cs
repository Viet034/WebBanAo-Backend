namespace WebBanAoo.Models;

public class Cart
{
    public int CartId { get; set; }
    public int CustomerId { get; set; }
    public string SessionId { get; set; }
    public virtual Customer Customer { get; set; }
    public virtual ICollection<Cart_ProductDetail> Cart_ProductDetails { get; set; } = new List<Cart_ProductDetail>();
}

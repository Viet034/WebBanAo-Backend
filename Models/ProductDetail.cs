using WebBanAoo.Models.ultility;
using static WebBanAoo.Models.Status.Status;

namespace WebBanAoo.Models;

public class ProductDetail : BaseEntity
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public ProductDetailStatus Status { get; set; } = ProductDetailStatus.Available;
    public int ProductId { get; set; }
    public virtual Product Product { get; set; }
    public int ColorId {  get; set; }
    public virtual Color Color { get; set; }
    public int SizeId {  get; set; }
    public virtual Size Size { get; set; }
    public virtual ICollection<ProductDetail_Sale> ProductDetail_Sales { get; set; } = new List<ProductDetail_Sale>();
    public virtual ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();
    public virtual ICollection<Cart_ProductDetail> Cart_ProductDetails { get; set; } = new List<Cart_ProductDetail>();
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}

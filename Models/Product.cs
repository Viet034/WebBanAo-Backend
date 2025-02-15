using WebBanAoo.Models.ultility;
using static WebBanAoo.Models.Status.Status;

namespace WebBanAoo.Models;

public class Product : BaseEntity
{

    public string ProductName { get; set; }
    public string Description { get; set; }
    public ProductStatus Status { get; set; } = ProductStatus.Available;
    public int CategoryId { get; set; }
    public int BrandId { get; set; }
    public virtual Category Category { get; set; } = null!;
    public virtual Brand Brand { get; set; } = null!;
    public virtual ICollection<ProductDetail> ProductDetails { get; set; } = new List<ProductDetail>();

}

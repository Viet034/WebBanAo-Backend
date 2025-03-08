using WebBanAoo.Ultility;

namespace WebBanAoo.Models;

public class Brand : BaseEntity
{
    public string BrandName { get; set; }
    public string? Image { get; set; }
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    
}

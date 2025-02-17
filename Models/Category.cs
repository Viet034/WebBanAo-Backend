using WebBanAoo.Ultility;
using static WebBanAoo.Models.Status.Status;

namespace WebBanAoo.Models;

public class Category : BaseEntity
{
    public string CategoryName { get; set; }
    public string Description { get; set; }
    public CategoryStatus Status { get; set; } = CategoryStatus.Active;
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}

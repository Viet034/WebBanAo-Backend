using WebBanAoo.Ultility;
using static WebBanAoo.Models.Status.Status;

namespace WebBanAoo.Models
{
    public class Color : BaseEntity
    {
        public string ColorName { get; set; }
        public ColorStatus Status { get; set; } = ColorStatus.Available;
        public virtual ICollection<ProductDetail> ProductDetails { get; set; } = new List<ProductDetail>();
    }
}

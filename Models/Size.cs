using WebBanAoo.Ultility;
using static WebBanAoo.Models.Status.Status;

namespace WebBanAoo.Models
{
    public class Size : BaseEntity
    {
        public SizeCode SizeCode { get; set; } = SizeCode.M;
        public SizeStatus Status { get; set; } = SizeStatus.Available;
        public virtual ICollection<ProductDetail> ProductDetails { get; set; } = new List<ProductDetail>();
    }
}

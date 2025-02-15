using WebBanAoo.Models.ultility;
using static WebBanAoo.Models.Status.Status;

namespace WebBanAoo.Models;

public class Sale : BaseEntity
{

    public string SaleName { get; set; }
    public SaleStatus Status { get; set; } = SaleStatus.Scheduled;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public virtual ICollection<ProductDetail_Sale> ProductDetail_Sales { get; set; } = new List<ProductDetail_Sale>();
}

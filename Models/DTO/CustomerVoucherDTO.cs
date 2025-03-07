using static WebBanAoo.Models.Status.Status;

namespace WebBanAoo.Models.DTO;

public class CustomerVoucherDTO
{
    public int Id { get; set; }
    public CustomerVoucherStatus Status { get; set; } = CustomerVoucherStatus.Active;
    public int CustomerId { get; set; }
    
    public int VoucherId { get; set; }
    
}

using WebBanAoo.Ultility;
using static WebBanAoo.Models.Status.Status;

namespace WebBanAoo.Models;

public class Customer : BaseEntity
{
    public string FullName { get; set; }
    public DateTime Dob {  get; set; }
    public Gender Gender { get; set; } = Gender.Male;
    public string Email { get; set; }
    public string Password { get; set; }
    public string Phone { get; set; }
    public CustomerStatus Status { get; set; } = CustomerStatus.Active;
    public string Address { get; set; }
    public string City { get; set; }
    public string? Image { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
    public string? ResetPasswordToken { get; set; }
    public DateTime? ResetPasswordTokenExpiry { get; set; }

    public virtual Cart Cart { get; set; }
    public virtual ICollection<Customer_Voucher> Customer_Vouchers { get; set; } = new List<Customer_Voucher>();
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}

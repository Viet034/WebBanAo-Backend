using System.ComponentModel.DataAnnotations;
using static WebBanAoo.Models.Status.Status;

namespace WebBanAoo.Models.DTO;

public class CustomerDTO
{
    public int Id { get; set; }
    [Required]
    public string Code { get; set; }
    [Required(ErrorMessage = "Customer Name is Missing")]
    public string FullName { get; set; }
    [Required]
    public DateTime Dob { get; set; }
    [EnumDataType(typeof(Gender))]
    public Gender Gender { get; set; } = Gender.Male;
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    public string Phone { get; set; }
    [EnumDataType(typeof(CustomerStatus))]
    public CustomerStatus Status { get; set; } = CustomerStatus.Active;
    [Required]
    public string Address { get; set; }
    [Required]
    public string City { get; set; }
    public string? Image { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
}

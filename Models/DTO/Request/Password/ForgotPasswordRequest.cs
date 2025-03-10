using System.ComponentModel.DataAnnotations;

namespace WebBanAoo.Models.DTO.Request.Password;

public class ForgotPasswordRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public UserType UserType { get; set; }
}

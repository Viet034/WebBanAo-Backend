using System.ComponentModel.DataAnnotations;

namespace WebBanAoo.Models.DTO.Request.Password;

public class ResetPasswordRequest
{
    [Required]
    public string Token { get; set; }

    [Required]
    [MinLength(6)]
    public string NewPassword { get; set; }

    [Required]
    public UserType UserType { get; set; }
}

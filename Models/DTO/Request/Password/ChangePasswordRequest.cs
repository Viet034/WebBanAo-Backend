using System.ComponentModel.DataAnnotations;

namespace WebBanAoo.Models.DTO.Request.Password;

public class ChangePasswordRequest
{
    [Required]
    public string OldPassword { get; set; }

    [Required]
    [MinLength(6)]
    public string NewPassword { get; set; }
}

using System.ComponentModel.DataAnnotations;

namespace WebBanAoo.Models.DTO;

public class CartDTO
{
    public int CartId { get; set; }
    [Required]
    public int CustomerId { get; set; }
    [Required]
    public string SessionId { get; set; }
}

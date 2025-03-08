using System.ComponentModel.DataAnnotations;

namespace WebBanAoo.Models.DTO;

public class BrandDTO
{
    
    public int Id { get; set; }
    [Required]
    public string Code { get; set; }
    [Required]
    public string BrandName { get; set; }
    public string? Image { get; set; }
}

using System.ComponentModel.DataAnnotations;
using static WebBanAoo.Models.Status.Status;

namespace WebBanAoo.Models.DTO;

public class ColorDTO
{
    public int Id { get; set; }
    [Required]
    public string Code { get; set; }
    [Required(ErrorMessage = "Color Name is Missing")]
    public string ColorName { get; set; }
    [EnumDataType(typeof(ColorStatus))]
    public ColorStatus Status { get; set; } = ColorStatus.Available;
}

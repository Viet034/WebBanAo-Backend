using System.ComponentModel.DataAnnotations;
using static WebBanAoo.Models.Status.Status;

namespace WebBanAoo.Models.DTO;

public class SizeDTO
{
    public int Id { get; set; }
    [Required]
    public string Code { get; set; }
    [EnumDataType(typeof(SizeStatus))]
    public SizeStatus Status { get; set; } = SizeStatus.Available;

    [EnumDataType(typeof(SizeCode))]
    public SizeCode SizeCode { get; set; } = SizeCode.M;
}

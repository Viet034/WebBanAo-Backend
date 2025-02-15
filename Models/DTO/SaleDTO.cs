using System.ComponentModel.DataAnnotations;
using static WebBanAoo.Models.Status.Status;

namespace WebBanAoo.Models.DTO;

public class SaleDTO
{
    public int Id { get; set; }
    [Required]
    public string Code { get; set; }
    [Required(ErrorMessage = "Sale Name is Missing")]
    public string SaleName { get; set; }
    [EnumDataType(typeof(SaleStatus))]
    public SaleStatus Status { get; set; } = SaleStatus.Scheduled;
    [Required]
    public DateTime StartDate { get; set; }
    [Required]
    public DateTime EndDate { get; set; }
}

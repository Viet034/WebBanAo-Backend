using System.ComponentModel.DataAnnotations;
using static WebBanAoo.Models.Status.Status;

namespace WebBanAoo.Models.DTO;

public class EmployeeDTO
{
    public int Id { get; set; }
    [Required]
    public string Code { get; set; }
    [Required(ErrorMessage = "Employee Name is Missing")]
    public string FullName { get; set; }
    [EnumDataType(typeof(EmployeeStatus))]
    public EmployeeStatus Status { get; set; } = EmployeeStatus.Working;
    [Required]
    public DateTime Dob { get; set; }
    [EnumDataType(typeof(Gender))]
    public Gender Gender { get; set; } = Gender.Male;
    public string Image { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    public string Phone { get; set; }
    [Required]
    public string Address { get; set; }
    [Required]
    public string Country { get; set; }
    [Required]
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}

using static WebBanAoo.Models.Status.Status;
using System.ComponentModel.DataAnnotations;

namespace WebBanAoo.Models.DTO.Request.Employee;

public class EmployeeRegisterRequest
{
    [Required(ErrorMessage = "Họ tên không được để trống")]
    public string FullName { get; set; }

    [Required(ErrorMessage = "Email không được để trống")]
    [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Mật khẩu không được để trống")]
    [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Số điện thoại không được để trống")]
    [Phone(ErrorMessage = "Số điện thoại không đúng định dạng")]
    public string Phone { get; set; }

    public DateTime Dob { get; set; }
    public Gender Gender { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public List<string> RoleIds { get; set; }
}

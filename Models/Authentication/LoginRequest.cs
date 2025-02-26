using System.ComponentModel.DataAnnotations;

public class LoginRequest
{
    [Required(ErrorMessage = "Email không được để trống")]
    [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Mật khẩu không được để trống")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Loại người dùng không được để trống")]
    public UserType UserType { get; set; }
}

public enum UserType
{
    Customer,
    Employee
}
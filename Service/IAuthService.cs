using WebBanAoo.Models.DTO.Request.Customer;
using WebBanAoo.Models.DTO.Request.Employee;
using WebBanAoo.Models.DTO.Response;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest request);
    Task<LoginResponse> RefreshTokenAsync(string refreshToken);
    Task<CustomerResponse> RegisterCustomerAsync(CustomerRegisterRequest request);
    Task<EmployeeResponse> RegisterEmployeeAsync(EmployeeRegisterRequest request);

    Task<bool> LogoutAsync(int userId, UserType userType);
    Task<bool> ChangePasswordAsync(int userId, UserType userType, string oldPassword, string newPassword);
    Task<bool> ForgotPasswordAsync(string email, UserType userType);
    Task<bool> ResetPasswordAsync(string token, string newPassword, UserType userType);
} 
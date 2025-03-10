namespace WebBanAoo.Service;

public interface IEmailService
{
    Task SendResetPasswordEmailAsync(string toEmail, string resetToken, string userType);
}

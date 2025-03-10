﻿using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace WebBanAoo.Service.impl;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendResetPasswordEmailAsync(string toEmail, string resetToken, string userType)
    {
        var smtpSettings = _configuration.GetSection("SmtpSettings");
        var fromEmail = smtpSettings["FromEmail"];
        var host = smtpSettings["Host"];
        var port = int.Parse(smtpSettings["Port"]);
        var username = smtpSettings["Username"];
        var password = smtpSettings["Password"];
        var websiteUrl = smtpSettings["WebsiteUrl"];

        using var client = new SmtpClient
        {
            Host = host,
            Port = port,
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(username, password)
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(fromEmail, "Colo Shop"),
            Subject = "Đặt lại mật khẩu",
            IsBodyHtml = true,
            Body = $@"
                    <h2>Đặt lại mật khẩu</h2>
                    <p>Bạn đã yêu cầu đặt lại mật khẩu cho tài khoản tại Colo Shop.</p>
                    <p>Click vào link bên dưới để đặt lại mật khẩu của bạn:</p>
                    <a href='{websiteUrl}/reset-password.html?token={resetToken}&userType={userType}'>
                        Đặt lại mật khẩu
                    </a>
                    <p>Link này sẽ hết hạn sau 24 giờ.</p>
                    <p>Nếu bạn không yêu cầu đặt lại mật khẩu, vui lòng bỏ qua email này.</p>"
        };
        mailMessage.To.Add(toEmail);

        try
        {
            await client.SendMailAsync(mailMessage);
        }
        catch (Exception ex)
        {
            throw new Exception($"Lỗi gửi email: {ex.Message}");
        }
    }
}

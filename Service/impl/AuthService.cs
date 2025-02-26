using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebBanAoo.Data;
using WebBanAoo.Models;
using WebBanAoo.Service;
using WebBanAoo.Service.impl;
using System.Security.Cryptography;
using static WebBanAoo.Models.Status.Status;
using WebBanAoo.Models.DTO.Response;
using WebBanAoo.Models.DTO.Request.Customer;
using WebBanAoo.Models.DTO.Request.Employee;

namespace Service.impl
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ICustomPasswordHasher _customerPasswordHasher;

        public AuthService(
            ApplicationDbContext context,
            IConfiguration configuration,
            ICustomPasswordHasher customerPasswordHasher)
        {
            _context = context;
            _configuration = configuration;
            _customerPasswordHasher = customerPasswordHasher;
        }

        public Task<bool> ChangePasswordAsync(int userId, UserType userType, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            object user = request.UserType switch
            {
                UserType.Customer => await _context.Customers
                    .FirstOrDefaultAsync(x => x.Email == request.Email),
                UserType.Employee => await _context.Employees
                    .Include(e => e.Employee_Roles)
                    .ThenInclude(er => er.Role)
                    .FirstOrDefaultAsync(x => x.Email == request.Email),
                _ => throw new ArgumentException("Invalid user type")
            };

            if (user == null)
            {
                throw new Exception("Email hoặc mật khẩu không đúng");
            }

            string password = request.UserType == UserType.Customer
                ? ((Customer)user).Password
                : ((Employee)user).Password;

            if (!_customerPasswordHasher.VerifyPassword(request.Password, password))
            {
                throw new Exception("Email hoặc mật khẩu không đúng");
            }

            // Kiểm tra trạng thái tài khoản
            if (user is Employee employee && employee.Status != EmployeeStatus.Working)
            {
                throw new Exception("Tài khoản đã bị khóa");
            }
            if (user is Customer customer && customer.Status != CustomerStatus.Active)
            {
                throw new Exception("Tài khoản đã bị khóa");
            }

            // Tạo token và refresh token
            var token = GenerateJwtToken(user, request.UserType);
            var refreshToken = GenerateRefreshToken();

            // Lưu refresh token
            if (user is Employee emp)
            {
                emp.RefreshToken = refreshToken;
                emp.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            }
            else if (user is Customer cust)
            {
                cust.RefreshToken = refreshToken;
                cust.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            }

            await _context.SaveChangesAsync();

            return new LoginResponse
            {
                Token = token,
                RefreshToken = refreshToken,
                TokenExpiration = DateTime.UtcNow.AddHours(1),
                UserInfo = CreateUserInfo(user, request.UserType)
            };
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private UserInfo CreateUserInfo(object user, UserType userType)
        {
            var userInfo = new UserInfo();

            if (userType == UserType.Employee)
            {
                var employee = (Employee)user;
                userInfo.Id = employee.Id;
                userInfo.Code = employee.Code;
                userInfo.FullName = employee.FullName;
                userInfo.Email = employee.Email;
                userInfo.UserType = userType;
                userInfo.Roles = employee.Employee_Roles
                    .Select(er => er.Role.Name)
                    .ToList();
            }
            else
            {
                var customer = (Customer)user;
                userInfo.Id = customer.Id;
                userInfo.Code = customer.Code;
                userInfo.FullName = customer.FullName;
                userInfo.Email = customer.Email;
                userInfo.UserType = userType;
                userInfo.Roles = new List<string> { "Customer" };
            }

            return userInfo;
        }

        private string GenerateJwtToken(dynamic user, UserType userType)
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim("UserType", userType.ToString())
        };

            // Thêm roles cho Employee
            if (userType == UserType.Employee)
            {
                foreach (var employeeRole in user.Employee_Roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, employeeRole.Role.Name));
                }
            }
            else
            {
                claims.Add(new Claim(ClaimTypes.Role, "Customer"));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public Task<bool> LogoutAsync(int userId, UserType userType)
        {
            throw new NotImplementedException();
        }

        public Task<LoginResponse> RefreshTokenAsync(string refreshToken)
        {
            throw new NotImplementedException();
        }

        public Task<CustomerResponse> RegisterCustomerAsync(CustomerRegisterRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<EmployeeResponse> RegisterEmployeeAsync(EmployeeRegisterRequest request)
        {
            throw new NotImplementedException();
        }

        // Implement các method khác...
    }
} 
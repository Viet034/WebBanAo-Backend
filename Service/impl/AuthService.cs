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
using WebBanAoo.Ultility;
using System.Text.RegularExpressions;

namespace Service.impl;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly ICustomPasswordHasher _customPasswordHasher;
    private readonly IEmailService _emailService;
    

    public AuthService(
        ApplicationDbContext context,
        IConfiguration configuration,
        ICustomPasswordHasher customPasswordHasher,
        IEmailService emailService)
    {
        _context = context;
        _configuration = configuration;
        _customPasswordHasher = customPasswordHasher;
        _emailService = emailService;
    }

    public async Task<bool> ChangePasswordAsync(int userId, UserType userType, string oldPassword, string newPassword)
    {
        if (userType == UserType.Employee)
        {
            var employee = await _context.Employees.FindAsync(userId);
            if (employee == null || !_customPasswordHasher.VerifyPassword(oldPassword, employee.Password))
            {
                throw new Exception("Mật khẩu cũ không đúng");
            }
            employee.Password = _customPasswordHasher.HashPassword(newPassword);
        }
        else
        {
            var customer = await _context.Customers.FindAsync(userId);
            if (customer == null || !_customPasswordHasher.VerifyPassword(oldPassword, customer.Password))
            {
                throw new Exception("Mật khẩu cũ không đúng");
            }
            customer.Password = _customPasswordHasher.HashPassword(newPassword);
        }

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        object user = request.UserType switch
    {
        UserType.Customer => await _context.Customers
            .FirstOrDefaultAsync(x => x.Email == request.Email),
        UserType.Employee => await _context.Employees
            .Include(e => e.Employee_Roles)  // Đảm bảo load roles
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

        if (!_customPasswordHasher.VerifyPassword(request.Password, password))
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

    // Thêm role cho Employee
    if (userType == UserType.Employee)
    {
        // Thêm role mặc định "Employee"
        claims.Add(new Claim(ClaimTypes.Role, "Employee"));
        
        // Nếu có Employee_Roles thì thêm vào
        if (user.Employee_Roles != null)
        {
            foreach (var employeeRole in user.Employee_Roles)
            {
                if (employeeRole.Role != null)
                {
                    claims.Add(new Claim(ClaimTypes.Role, employeeRole.Role.Name));
                }
            }
        }
    }
    else
    {
        claims.Add(new Claim(ClaimTypes.Role, "Customer"));
    }

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    var expires = DateTime.UtcNow.AddHours(Convert.ToDouble(_configuration["Jwt:TokenExpirationInHours"]));
    
    var token = new JwtSecurityToken(
        issuer: _configuration["Jwt:Issuer"],
        audience: _configuration["Jwt:Audience"],
        claims: claims,
        expires: expires,
        signingCredentials: creds
    );

    return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<bool> LogoutAsync(int userId, UserType userType)
    {
        try
        {
            if (userType == UserType.Employee)
            {
                var employee = await _context.Employees.FindAsync(userId);
                if (employee != null)
                {
                    // Xóa refresh token khi đăng xuất
                    employee.RefreshToken = null;
                    employee.RefreshTokenExpiryTime = null;
                }
                else
                {
                    return false;
                }
            }
            else if (userType == UserType.Customer)
            {
                var customer = await _context.Customers.FindAsync(userId);
                if (customer != null)
                {
                    // Xóa refresh token khi đăng xuất
                    customer.RefreshToken = null;
                    customer.RefreshTokenExpiryTime = null;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<LoginResponse> RefreshTokenAsync(string refreshToken)
    {
        // Tìm user với refresh token
        var customer = await _context.Customers
            .FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);
        var employee = await _context.Employees
            .Include(e => e.Employee_Roles)
            .ThenInclude(er => er.Role)
            .FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);

        if (customer == null && employee == null)
        {
            throw new Exception("Invalid refresh token");
        }

        object user;
        UserType userType;
        DateTime? tokenExpiry;

        if (customer != null)
        {
            user = customer;
            userType = UserType.Customer;
            tokenExpiry = customer.RefreshTokenExpiryTime;
        }
        else
        {
            user = employee;
            userType = UserType.Employee;
            tokenExpiry = employee.RefreshTokenExpiryTime;
        }

        // Kiểm tra token hết hạn
        if (tokenExpiry == null || tokenExpiry < DateTime.UtcNow)
        {
            throw new Exception("Refresh token expired");
        }

        // Tạo token mới
        var newToken = GenerateJwtToken(user, userType);
        var newRefreshToken = GenerateRefreshToken();

        // Cập nhật refresh token
        if (userType == UserType.Customer)
        {
            customer.RefreshToken = newRefreshToken;
            customer.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        }
        else
        {
            employee.RefreshToken = newRefreshToken;
            employee.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        }

        await _context.SaveChangesAsync();

        return new LoginResponse
        {
            Token = newToken,
            RefreshToken = newRefreshToken,
            TokenExpiration = DateTime.UtcNow.AddHours(1),
            UserInfo = CreateUserInfo(user, userType)
        };
    }

    public async Task<CustomerResponse> RegisterCustomerAsync(CustomerRegisterRequest request)
    {
        // Kiểm tra email đã tồn tại
        if (await _context.Customers.AnyAsync(x => x.Email == request.Email))
        {
            throw new Exception("Email đã tồn tại");
        }
        if (await _context.Customers.AnyAsync(x => x.Phone == request.Phone))
        {
            throw new Exception("Số điện thoại đã được sử dụng");
        }
        request.FullName = request.FullName.Trim();
        if (string.IsNullOrEmpty(request.FullName))
        {
            throw new Exception("Không được để trống tên");
        }
        if(!Regex.IsMatch(request.FullName, @"^[a-zA-ZÀ-Ỹà-ỹ\s]+$"))
        {
            throw new Exception("Tên không được chứa kí tự đặc biệt");
        }
        
        // Hash password
        var hashedPassword = _customPasswordHasher.HashPassword(request.Password);

        // Tạo customer mới
        var customer = new Customer
        {
            Code = GenerateCode.GenerateCustomerCode(),
            FullName = request.FullName,
            Email = request.Email,
            Password = hashedPassword,
            Phone = request.Phone,
            Address = request.Address,
            City = request.City,
            Dob = request.Dob,
            Gender = Gender.Male,
            Status = CustomerStatus.Active,
            CreateDate = DateTime.Now.AddHours(7),
            UpdateDate = DateTime.Now.AddHours(7),
            CreatedBy = request.FullName,
            Image = ""

        };

        await _context.Customers.AddAsync(customer);
        await _context.SaveChangesAsync();

        return new CustomerResponse
        {
            Id = customer.Id,
            Code = customer.Code,
            FullName = customer.FullName,
            Email = customer.Email,
            Phone = customer.Phone,
            Address = customer.Address,
            City = customer.City,
            Dob = customer.Dob,
            Gender = customer.Gender,
            Status = customer.Status,
            
        };
    }

    public async Task<EmployeeResponse> RegisterEmployeeAsync(EmployeeRegisterRequest request)
    {
        // Kiểm tra email đã tồn tại
        if (await _context.Employees.AnyAsync(x => x.Email == request.Email))
        {
            throw new Exception("Email đã tồn tại");
        }
        if (await _context.Employees.AnyAsync(x => x.Phone == request.Phone))
        {
            throw new Exception("Số điện thoại đã được sử dụng");
        }
        request.FullName = request.FullName.Trim();
        if (string.IsNullOrEmpty(request.FullName))
        {
            throw new Exception("Không được để trống tên");
        }
        if (!Regex.IsMatch(request.FullName, @"^[a-zA-ZÀ-Ỹà-ỹ\s]+$"))
        {
            throw new Exception("Tên không được chứa kí tự đặc biệt");
        }

        // Hash password
        var hashedPassword = _customPasswordHasher.HashPassword(request.Password);
        // Tạo customer mới
        var emp = new Employee
        {
            Code = GenerateCode.GenerateEmployeeCode(),
            FullName = request.FullName,
            Email = request.Email,
            Password = hashedPassword,
            Phone = request.Phone,
            Address = request.Address,
            City = request.City,
            Dob = request.Dob,
            Gender = Gender.Male,
            Status = EmployeeStatus.Working,
            CreateDate = DateTime.Now.AddHours(7),
            UpdateDate = DateTime.Now.AddHours(7),
            CreatedBy = request.FullName,
            Image = ""
        };
        await _context.Employees.AddAsync(emp);
        await _context.SaveChangesAsync();

        return new EmployeeResponse
        {
            Id = emp.Id,
            Code = emp.Code,
            FullName = emp.FullName,
            Email = emp.Email,
            
            Phone = emp.Phone,
            Address = emp.Address,
            City = emp.City,
            Dob = emp.Dob,
            Gender = emp.Gender,
            Status = emp.Status,
            
        };
    }

    public async Task<bool> ForgotPasswordAsync(string email, UserType userType)
    {
        try
        {
            // 1. Tìm user theo email và chỉ định kiểu object
            object user = userType switch
            {
                UserType.Employee => await _context.Employees
                    .FirstOrDefaultAsync(e => e.Email == email),
                UserType.Customer => await _context.Customers
                    .FirstOrDefaultAsync(c => c.Email == email),
                _ => throw new ArgumentException("Invalid user type")
            };

            if (user == null) throw new Exception("Email không tồn tại"); ;

            // 2. Tạo reset token
            var resetToken = Guid.NewGuid().ToString();
            var resetTokenExpiry = DateTime.UtcNow.AddHours(24);

            // 3. Lưu token vào database
            if (user is Employee employee)
            {
                employee.ResetPasswordToken = resetToken;
                employee.ResetPasswordTokenExpiry = resetTokenExpiry;
            }
            else if (user is Customer customer)
            {
                customer.ResetPasswordToken = resetToken;
                customer.ResetPasswordTokenExpiry = resetTokenExpiry;
            }

            await _context.SaveChangesAsync();

            // 4. Gửi email reset password
            await _emailService.SendResetPasswordEmailAsync(email, resetToken, userType.ToString());

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> ResetPasswordAsync(string token, string newPassword, UserType userType)
    {
        // 1. Tìm user theo token và chỉ định kiểu object
        object user = userType switch
        {
            UserType.Employee => await _context.Employees
                .FirstOrDefaultAsync(e => e.ResetPasswordToken == token
                    && e.ResetPasswordTokenExpiry > DateTime.UtcNow),
            UserType.Customer => await _context.Customers
                .FirstOrDefaultAsync(c => c.ResetPasswordToken == token
                    && c.ResetPasswordTokenExpiry > DateTime.UtcNow),
            _ => throw new ArgumentException("Invalid user type")
        };

        if (user == null) throw new Exception("Token không hợp lệ hoặc đã hết hạn");
        

        // 2. Set mật khẩu mới
        var hashedPassword = _customPasswordHasher.HashPassword(newPassword);

        if (user is Employee employee)
        {
            employee.Password = hashedPassword;
            employee.ResetPasswordToken = null;
            employee.ResetPasswordTokenExpiry = null;
        }
        else if (user is Customer customer)
        {
            customer.Password = hashedPassword;
            customer.ResetPasswordToken = null;
            customer.ResetPasswordTokenExpiry = null;
        }

        await _context.SaveChangesAsync();
        return true;
    }

    // Implement các method khác...
}

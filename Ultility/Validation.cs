using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using WebBanAoo.Data;

namespace WebBanAoo.Ultility;

public class Validation<T> where T : class
{
    private readonly ApplicationDbContext _context;
    private static readonly Regex EmailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);
    private static readonly Regex PasswordRegex = new Regex(@"^(?=.*[A-Z])(?=.*[\W_]).{6,}$", RegexOptions.Compiled);
    private static readonly Regex PhoneRegex = new Regex(@"^0\d{9}$", RegexOptions.Compiled);


    public Validation(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<bool> IsValidPhone(string phone)
    {
        return !string.IsNullOrEmpty(phone) && PhoneRegex.IsMatch(phone);
    }
    public async Task<bool> IsValidEmail(string email)
    {
        return !string.IsNullOrEmpty(email) && EmailRegex.IsMatch(email);
    }

    public async Task<bool> IsValidPassword(string password)
    {
        return !string.IsNullOrEmpty(password) && PasswordRegex.IsMatch(password);
    }

    public async Task<string> ValidateAndUpdateAsync(
    T entity,
    string currentValue,
    string newValue,
    Expression<Func<T, bool>> predicate,
    bool isPhone = false,
    bool isEmail = false,
    bool isPassword = false)
    {
        if (string.IsNullOrEmpty(newValue) || newValue == "string" || newValue == currentValue)
        {
            return currentValue; // Giữ nguyên giá trị cũ nếu không thay đổi
        }

        if (isPhone && !PhoneRegex.IsMatch(newValue))
        {
            throw new Exception("Số điện thoại không hợp lệ! Phải có 10 chữ số và bắt đầu bằng 0.");
        }

        if (isEmail && !EmailRegex.IsMatch(newValue))
        {
            throw new Exception("Email không hợp lệ!");
        }

        if (isPassword && !PasswordRegex.IsMatch(newValue))
        {
            throw new Exception("Mật khẩu không hợp lệ!");
        }

        bool exist = await _context.Set<T>().AnyAsync(predicate);
        if (exist)
        {
            throw new Exception($"Giá trị {newValue} đã tồn tại!");
        }

        return newValue; // Cập nhật với giá trị hợp lệ
    }

    public async Task<bool> IsUniqueAsync(Expression<Func<T, bool>> predicate)
    {
        return !await _context.Set<T>().AnyAsync(predicate);
    }

    public async Task<string> CheckAndUpdateAPIAsync(T entity, string currentValue, string newValue, Expression<Func<T, bool>> predicate)
    {
        if(!string.IsNullOrEmpty(newValue) && newValue != "string" && newValue != currentValue)
        {
            bool exist = await _context.Set<T>().AnyAsync(predicate);
            if(exist)
            {
                throw new Exception($"Giá trị {newValue} đã tồn tại!");
            }
            return newValue;
        }
        return currentValue;
    }
    public async Task<decimal> CheckAndUpdatePriceAsync<T>(T entity, decimal currentValue, decimal newValue, Func<T, bool> predicate)
    {
        if (predicate(entity))
        {
            return newValue;
        }
        return currentValue;
    }

    public async Task<int> CheckAndUpdateQuantityAsync<T>(T entity, int currentValue, int newValue, Func<T, bool> predicate)
    {
        if (predicate(entity))
        {
            return newValue;
        }
        return currentValue;
    }

    public async Task<DateTime> CheckAndUpdateDateGeneralAsync(T entity, DateTime oldValue, DateTime? newValue, DateTime? otherDate, bool isStartDate)
    {
        if (newValue.HasValue && newValue.Value != oldValue)
        {
            if (isStartDate && otherDate.HasValue && newValue.Value > otherDate.Value)
            {
                throw new Exception("Ngày bắt đầu không thể diễn ra sau ngày kết thúc.");
            }
            if (!isStartDate && otherDate.HasValue && newValue.Value < otherDate.Value)
            {
                throw new Exception("Ngày kết thúc không thể diễn ra trước ngày bắt đầu.");
            }
            return newValue.Value;
        }
        return oldValue;
    }
    public async Task<DateTime?> CheckAndUpdateDateEmployeeAsync(T entity,DateTime? currentValue,DateTime? newValue,DateTime? existingValue,bool someFlag)
    {
        if (!newValue.HasValue)
        {
            return currentValue; // Giữ nguyên nếu newValue là null
        }

        return newValue; // Cập nhật nếu có giá trị
    }

    public async Task<DateTime> CheckAndUpdateDOBGeneralAsync(T entity, DateTime oldValue, DateTime? newValue)
    {
        if (newValue.HasValue && newValue.Value != oldValue)
        {
            if (newValue.Value > DateTime.Today)
            {
                throw new Exception("Năm sinh không thể lớn hơn năm hiện tại.");
            }
            if (newValue.Value.Year < 1900)
            {
                throw new Exception("Ngày sinh không hợp lệ.");
            }
            return newValue.Value;
        }
        return oldValue;
    }

}

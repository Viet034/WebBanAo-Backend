using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebBanAoo.Data;

namespace WebBanAoo.Models.Ultility;

public class Validation<T> where T : class
{
    private readonly ApplicationDbContext _context;

    public Validation(ApplicationDbContext context)
    {
        _context = context;
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

    public async Task<DateTime> CheckAndUpdateDateAsync(T entity, DateTime oldValue, DateTime? newValue, DateTime? otherDate, bool isStartDate)
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
    public async Task<DateTime> CheckAndUpdateDOBAsync(T entity, DateTime oldValue, DateTime? newValue)
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

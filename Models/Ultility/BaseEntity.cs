using Microsoft.EntityFrameworkCore;

namespace WebBanAoo.Models.ultility;

public class BaseEntity
{
    public int Id { get; set; }
    public string Code { get; set; }
    public DateTime CreateDate { get; set; } = DateTime.UtcNow;
    public DateTime? UpdateDate { get; set; }
    public string CreatedBy { get; set; }
    public string? UpdateBy { get; set; }
}
